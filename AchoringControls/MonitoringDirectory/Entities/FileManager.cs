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
using MonitoringDirectory.Interfaces;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MonitoringDirectory.Entities
{
    public abstract class FileManager : Interfaces.IFileProvider
    {
        public DirectoryInfo SourceDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        public FileManager(string sourcePath)
        {
            if (!sourcePath.EndsWith(@"\"))
                sourcePath = sourcePath + @"\";
            this.SourceDirectory = new DirectoryInfo(sourcePath);
            if (!SourceDirectory.Exists)
            {
                SourceDirectory.Create();
            }

            ConfigureFileWatcher();
        }

        private void ConfigureFileWatcher()
        {
            fileWatcher.Path = this.SourceDirectory.FullName;
            fileWatcher.Filter = "*";

            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            Observable
                .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Changed")
                .Synchronize()
                .Select(e => e.EventArgs)
                .Buffer(TimeSpan.FromMilliseconds(2000))
                .Subscribe(files =>
                {
                    foreach (var file in files)
                    {
                        if (!file.Name.StartsWith("~"))
                        {
                            OnChanged(new File(file.Name, file.FullPath));
                        }
                    }
                });

            Observable
               .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Deleted")
               .Synchronize()
               .Select(e => e.EventArgs)
               .Buffer(TimeSpan.FromMilliseconds(2000))
               .Subscribe(files =>
               {
                   foreach (var file in files)
                   {
                       OnDeleted(new File(file.Name, file.FullPath));
                   }
               });

            Observable
              .FromEventPattern<RenamedEventArgs>(fileWatcher, "Renamed")
              .Synchronize()
              .Select(e => e.EventArgs)
              .Buffer(TimeSpan.FromMilliseconds(2000))
              .Subscribe(files =>
              {
                  foreach (var file in files)
                  {
                      if (!file.Name.StartsWith("~"))
                      {
                          OnRenamed(new File(file.OldName, file.OldFullPath), new File(file.Name, file.FullPath));
                      }
                  }
              });

            fileWatcher.EnableRaisingEvents = true;
        }

        public virtual ICollection<Entities.File> Files { get; set; }

        public virtual IList<File> GetFiles(string path)
        {
            IList<File> fileList = new List<File>();
            foreach (var file in new DirectoryInfo(path).EnumerateFiles())
            {
                var newFile = new File(file.Name, file.DirectoryName, file.Length);
                newFile.LastChange = file.LastWriteTime;
                fileList.Add(newFile);
            }
            return fileList;
        }

        public virtual void DeleteFile(File file)
        {
            System.IO.File.Delete(file.FullPath());
        }

        public virtual void OpenFile(File file)
        {
            Process.Start(file.FullPath());
        }

        async virtual public Task CopyFileTo(File file, string destination)
        {
            await Task.Run(() => System.IO.File.Copy(file.FullPath(), Path.Combine(destination, file.Name), true));
        }

        public abstract void OnDeleted(File file);
        public abstract void OnChanged(File file);
        public abstract void OnRenamed(File oldFile, File file);
    }
}
