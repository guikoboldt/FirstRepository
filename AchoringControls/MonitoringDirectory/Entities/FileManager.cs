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
using Polly;

namespace MonitoringDirectory.Entities
{
    public abstract class FileManager : Interfaces.IFileProvider
    {
        public DirectoryInfo SourceDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        private Policy filePolicys { get; set; }

        public FileManager(string sourcePath)
        {
            if (!sourcePath.EndsWith(@"\"))
                sourcePath = sourcePath + @"\";
            this.SourceDirectory = new DirectoryInfo(sourcePath);
            if (!SourceDirectory.Exists)
            {
                SourceDirectory.Create();
            }

            filePolicys = Policy
              .Handle<IOException>()
              .WaitAndRetry(5, retryAttempt => (TimeSpan.FromSeconds(15 * retryAttempt)));
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
                            var name = file.Name;
                            var location = file.FullPath;
                            foreach (var invalidString in new string[] { @"[(]\d+[)].\w+", @"[.]\w+" })
                            {
                                name = Regex.Replace(name, invalidString, "");
                                location = Regex.Replace(location, name + invalidString, "");
                            }
                            OnChanged(new File(file.Name, location));
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
                       var name = file.Name;
                       var location = file.FullPath;
                       foreach (var invalidString in new string[] { @"[(]\d+[)].\w+", @"[.]\w+" })
                       {
                           name = Regex.Replace(name, invalidString, "");
                           location = Regex.Replace(location, name + invalidString, "");
                       }
                       OnDeleted(new File(file.Name, location));
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
                          var oldName = file.OldName;
                          var oldLocation = file.OldFullPath;
                          var name = file.Name;
                          var location = file.FullPath;
                          foreach (var invalidString in new string[] { @"[(]\d+[)].\w+", @"[.]\w+" })
                          {
                              name = Regex.Replace(name, invalidString, "");
                              location = Regex.Replace(location, name + invalidString, "");
                              oldName = Regex.Replace(oldName, invalidString, "");
                              oldLocation = Regex.Replace(oldLocation, invalidString, "");
                          }
                          OnRenamed(new File(file.OldName, oldLocation), new File(file.Name, location));
                      }
                  }
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
            filePolicys.Execute(() =>
            {
                System.IO.File.Delete(file.FullPath());
            });
        }

        public virtual void OpenFile(File file)
        {
            filePolicys.Execute(() =>
            {
                Process.Start(file.FullPath());
            });
        }

        virtual public void CopyFileTo(File file, string destination)
        {
              filePolicys.Execute(() => 
                {
                    System.IO.File.Copy(file.FullPath() ,Path.Combine(destination, file.Name), true);
                });
        }

        public abstract void OnDeleted(File file);
        public abstract void OnChanged(File file);
        public abstract void OnRenamed(File oldFile, File file);
    }
}
