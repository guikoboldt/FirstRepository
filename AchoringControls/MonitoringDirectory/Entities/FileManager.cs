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

namespace MonitoringDirectory.Entities
{
    public class FileManager : Interfaces.IFileProvider
    {
        public DirectoryInfo sourceDirectory { get; set; }

        public DirectoryInfo targetDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();
        public ObservableCollection<FileInfo> TargetFiles { get; private set; } = new ObservableCollection<FileInfo>();

        public FileManager(string sourcePath, string targetPath)
        {
            this.sourceDirectory = new DirectoryInfo(sourcePath);
            if (!sourceDirectory.Exists)
            {
                sourceDirectory.Create();
            }
            this.targetDirectory = new DirectoryInfo(targetPath);
            if (!targetDirectory.Exists)
            {
                targetDirectory.Create();
            }

            LoadAllFiles();
            ConfigureFileWatcher();
        }

        private void ConfigureFileWatcher()
        {
            fileWatcher.Path = this.sourceDirectory.FullName;
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
                            var changedFile = (from foundFile in TargetFiles
                                               where foundFile.Name.Equals(file.Name)
                                               select foundFile).FirstOrDefault();

                            if (changedFile != null)
                            {
                                DeleteFile(changedFile, targetDirectory.FullName);
                                TargetFiles.Remove(changedFile);
                            }
                            var newFile = new FileInfo(file.FullPath);
                            await CopyFileAsync(newFile, targetDirectory.FullName);
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
                       var deletedFile = (from foundFile in TargetFiles
                                          where foundFile.Name.Equals(file.Name)
                                          select foundFile).FirstOrDefault();
                       if (deletedFile != null)
                       {
                           DeleteFile(deletedFile, targetDirectory.FullName);
                           this.TargetFiles.Remove(deletedFile);
                       }
                   }
               });

            //Observable
            //  .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Created")
            //  .Synchronize()
            //  .Select(e => e.EventArgs)
            //  .Buffer(TimeSpan.FromMilliseconds(2000))
            //  .Subscribe(async files =>
            //  {
            //      foreach (var file in files)
            //      {
            //          var newFile = new FileInfo(file.FullPath);
            //          this.Files.Add(newFile);
            //          if (!newFile.Name.StartsWith("~"))
            //          {
            //              //await isFileAvailableAsync(newFile);
            //              await CopyFileAsync(newFile);
            //          }
            //      }
            //  });

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
                          var updatedFile = (from foundFile in TargetFiles
                                             where foundFile.Name.Equals(file.OldName)
                                             select foundFile).FirstOrDefault();
                          var newFile = new FileInfo(file.FullPath);
                          if (updatedFile != null)
                          {
                              DeleteFile(updatedFile, targetDirectory.FullName);
                              this.TargetFiles.Remove(updatedFile);
                          }
                          await CopyFileAsync(newFile, targetDirectory.FullName);
                          this.TargetFiles.Add(newFile);
                      }
                  }
              });

            fileWatcher.EnableRaisingEvents = true;
        }

        public void OpenFile(FileInfo file)
        {
            System.Diagnostics.Process.Start(file.FullName);
        }

        private void LoadAllFiles()
        {
            this.TargetFiles.Clear();
            foreach (var file in this.targetDirectory.EnumerateFiles())
            {
                this.TargetFiles.Add(file);
            }
        }

        public void DeleteFile(FileInfo deletedFile, string path)
        {
            var file = new FileInfo(Path.Combine(path, deletedFile.Name));
            if (File.Exists(file.FullName))
            {
                file.Delete();
            }
        }

        async public Task CopyFileAsync(FileInfo file, string targetPath)
        {
            try
            {
                file.CopyTo(Path.Combine(targetPath, file.Name), true);
            }
            catch (IOException)
            {
                await CopyFileAsync(file, targetPath);
            }
        }

        File[] IFileProvider.Files(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(File file)
        {
            throw new NotImplementedException();
        }

        public void OpenFile(File file)
        {
            throw new NotImplementedException();
        }

        public void CopyFileTo(File file, string destination)
        {
            throw new NotImplementedException();
        }
        
        public File CreateFile(string name, int size, string location)
        {
            throw new NotImplementedException();
        }
    }
}
