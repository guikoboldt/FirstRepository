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

namespace MonitoringDirectory.Entities
{
    public class FileManager : Interfaces.IFileProvider
    {
        public DirectoryInfo SourceDirectory { get; set; }

        public DirectoryInfo TargetDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        public FileManager(string sourcePath, string targetPath)
        {
            this.SourceDirectory = new DirectoryInfo(sourcePath);
            if (!SourceDirectory.Exists)
            {
                SourceDirectory.Create();
            }
            this.TargetDirectory = new DirectoryInfo(targetPath);
            if (!TargetDirectory.Exists)
            {
                TargetDirectory.Create();
            }

            this.Files = GetFiles(TargetDirectory.FullName);
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
                .Subscribe(async files =>
                {
                    foreach (var file in files)
                    {
                        if (!file.Name.StartsWith("~"))
                        {
                            var changedTargetFile = this.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
                            if (changedTargetFile != null)
                            {
                                DeleteFile(changedTargetFile);
                                Files.Remove(changedTargetFile);
                            }

                            var newFile = new File(file.Name, Regex.Replace(file.FullPath, file.Name, ""));
                            await CopyFileTo(newFile, TargetDirectory.FullName);
                            newFile.Location = TargetDirectory.FullName;
                            Files.Add(newFile);
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
                       var deletedFile = Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
                       if (deletedFile != null)
                       {
                           DeleteFile(deletedFile);
                           this.Files.Remove(deletedFile);
                       }
                   }
               });

            Observable
              .FromEventPattern<RenamedEventArgs>(fileWatcher, "Renamed")
              .Synchronize()
              .Select(e => e.EventArgs)
              .Buffer(TimeSpan.FromMilliseconds(2000))
              .Subscribe(async files =>
              {
                  foreach (var file in files)
                  {
                      if (!file.Name.StartsWith("~"))
                      {
                          var updatedFile = this.Files.FirstOrDefault(_ => _.Name.Equals(file.OldName));
                          if (updatedFile != null)
                          {
                              DeleteFile(updatedFile);
                              this.Files.Remove(updatedFile);
                          }
                          var newFile = new File(file.Name, Regex.Replace(file.FullPath, file.Name, ""));
                          await CopyFileTo(newFile, TargetDirectory.FullName);
                          newFile.Location = TargetDirectory.FullName;
                          this.Files.Add(newFile);
                      }
                  }
              });

            fileWatcher.EnableRaisingEvents = true;
        }

        public virtual ICollection<Entities.File> Files { get; private set; }

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
            throw new NotImplementedException();
        }

        async virtual public Task CopyFileTo(File file, string destination)
        {
            await Task.Run(() => System.IO.File.Copy(file.FullPath(), Path.Combine(destination, file.Name), true));
        }

    }
}
