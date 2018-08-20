using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CopyBackupFile.Interfaces;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Polly;

namespace CopyBackupFile.Entities
{
    public class FileManager : Interfaces.IFileProvider
    {
        public DirectoryInfo SourceDirectory { get; set; }
        public DirectoryInfo TargetDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        private Policy policy { get; set; }

        public FileManager(string sourcePath, string fileTypeForMonitoring, string targetPath, string user,
            string domain, string password)
        {
            if (!sourcePath.EndsWith(@"\"))
                sourcePath = sourcePath + @"\";
            this.SourceDirectory = new DirectoryInfo(sourcePath);
            if (!SourceDirectory.Exists)
            {
                SourceDirectory.Create();
            }

            try
            {
                using (new Helpers.Impersonator(user, domain, password))
                {
                    if (!targetPath.EndsWith(@"\"))
                        targetPath = targetPath + @"\";
                    this.TargetDirectory = new DirectoryInfo(targetPath);
                    if (!TargetDirectory.Exists)
                    {
                        TargetDirectory.Create();
                    }
                }
            }
            catch (Exception e)
            {
                Helpers.FileUtils.WriteLog(e, string.Format("Target path is not available, " +
                "Try to reconfigure the app! Exception: {0}", e.Message));
                return;
            }


            ConfigureFileWatcher(fileTypeForMonitoring, user, domain, password);
            Helpers.FileUtils.WriteLog(string.Format("Waiting for new backup file on {0}", sourcePath));
        }

        private void ConfigureFileWatcher(string fileTypeForMonitoring, string user, string domain, string password)
        {
            fileWatcher.Path = this.SourceDirectory.FullName;
            fileWatcher.Filter = fileTypeForMonitoring;

            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | 
                NotifyFilters.FileName;

            Observable
                .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Changed")
                .Select(e => e.EventArgs)
                .Throttle(TimeSpan.FromMinutes(5))
                .Do(e => Helpers.FileUtils.WriteLog(string.Format("Last Event of last 5 minutes: {0} {1} {2}",
                e.Name, e.FullPath, e.ChangeType)))
                .Subscribe(async file =>
                {
                    if (!file.Name.StartsWith("~"))
                    {
                        var name = file.Name;
                        var location = file.FullPath;
                        location = Regex.Replace(location, name, "");
                        await Helpers.FileUtils.TryCopyFile(new File(name, location),
                            TargetDirectory.FullName, user, domain, password);
                    }
                });

            Observable
               .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Deleted")
               .Select(e => e.EventArgs)
               .Throttle(TimeSpan.FromMinutes(5))
               .Do(e => Helpers.FileUtils.WriteLog(string.Format("Last Event of last 5 minutes: {0} {1} {2}",
                e.Name, e.FullPath, e.ChangeType)))
               .Subscribe(async file =>
               {
                   await Helpers.FileUtils.CheckFolderDestination(TargetDirectory.FullName,
                       user, domain, password);
               });

            fileWatcher.EnableRaisingEvents = true;
        }

        public virtual ICollection<Entities.File> Files { get; set; }

        public virtual ObservableCollection<File> GetFiles(string path)
        {
            ObservableCollection<File> fileList = new ObservableCollection<File>();
            foreach (var file in new DirectoryInfo(path).EnumerateFiles())
            {
                var newFile = new File(file.Name, SourceDirectory.FullName, file.Length);
                newFile.LastChange = file.LastWriteTime;
                fileList.Add(newFile);
            }
            return fileList;
        }

        public virtual void DeleteFile(File file)
        {
            Task.Factory.StartNew(() =>
            System.IO.File.Delete(file.FullPath()));
        }

        public virtual void OpenFile(File file)
        {
            Task.Run(() => Process.Start(file.FullPath()));
        }

        virtual public void CopyFileTo(File file, string destination)
        {
            Task.Run(() =>
              { System.IO.File.Copy(file.FullPath(), Path.Combine(destination, file.Name), true); });
        }
    }
}
