using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class FileManager
    {
        public DirectoryInfo sourceDirectory { get; set; }

        public DirectoryInfo targetDirectory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        //private FileSystemWatcher directoryWatcher = new FileSystemWatcher();
        public ObservableCollection<FileInfo> Files { get; private set; } = new ObservableCollection<FileInfo>();
        public ObservableCollection<string> Events { get; private set; } = new ObservableCollection<string>();

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
            //ConfigureDirectoryWatcher();
        }

        //private void ConfigureDirectoryWatcher()
        //{
        //    directoryWatcher.Path = sourceDirectory.Root.ToString();
        //    directoryWatcher.NotifyFilter = NotifyFilters.DirectoryName;
        //    directoryWatcher.IncludeSubdirectories = true;

        //    directoryWatcher.Renamed += (s, e) =>
        //    {
        //        if (e.OldFullPath.Equals(sourceDirectory.FullName))
        //        {
        //                Events.Insert(0, e.OldFullPath);
        //                Events.Insert(0, e.FullPath);
        //                sourceDirectory = new DirectoryInfo(e.FullPath);
        //                fileWatcher.Path = this.sourceDirectory.FullName;
        //                LoadAllFiles();
        //        }
        //    };

        //    directoryWatcher.EnableRaisingEvents = true;
        //}

        public void ConfigureFileWatcher()
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
                        var newFile = new FileInfo(file.FullPath);
                        if (!newFile.Name.StartsWith("~"))
                        {
                            var changedFile = (from foundFile in Files
                                               where foundFile.Name.Equals(newFile.Name)
                                               select foundFile).FirstOrDefault();
                            if (!newFile.Equals(changedFile))
                            {
                                this.Files.Remove(changedFile);
                                this.Files.Add(newFile);
                                File.Delete(Path.Combine(targetDirectory.FullName, changedFile.Name));
                                await TryCopyFile(newFile);
                            }
                        }
                    }
                });

            Observable
               .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Deleted")
               .Synchronize()
               .Select(e => e.EventArgs)
               .Subscribe(filePath =>
               {
                   var deletedFile = (from file in Files
                                      where file.Name.Equals(filePath.Name)
                                      select file).FirstOrDefault();
                   this.Files.Remove(deletedFile);
                   while (targetDirectory.GetFiles(deletedFile.Name).IsReadOnly) { }
                   File.Delete(Path.Combine(targetDirectory.FullName, deletedFile.Name));
               });

            Observable
              .FromEventPattern<FileSystemEventArgs>(fileWatcher, "Created")
              .Synchronize()
              .Select(e => e.EventArgs)
              .Buffer(TimeSpan.FromMilliseconds(2000))
              .Subscribe(async file =>
              {
                  foreach (var filePath in file)
                  {

                      var newFile = new FileInfo(filePath.FullPath);
                      this.Files.Add(newFile);
                      var complete = false;
                      while (!complete)
                      {
                          try
                          {
                              await TryCopyFile(newFile);
                              complete = true;
                          }
                          catch { }
                      }
                  }
              });

            Observable
              .FromEventPattern<RenamedEventArgs>(fileWatcher, "Renamed")
              .Synchronize()
              .Select(e => e.EventArgs)
              .Subscribe(async filePath =>
              {
                  var newFile = new FileInfo(filePath.FullPath);
                  var updatedFile = (from file in Files
                                     where file.Name.Equals(filePath.OldName)
                                     select file).FirstOrDefault();
                  this.Files.Remove(updatedFile);
                  this.Files.Add(newFile);
                  System.GC.Collect();
                  System.GC.WaitForPendingFinalizers();
                  File.Delete(Path.Combine(targetDirectory.FullName, updatedFile.Name));
                  await TryCopyFile(newFile);
              });


            //fileWatcher.Changed += (s, e) =>
            //{
            //    var newFile = new FileInfo(e.FullPath);
            //    if (!newFile.Name.StartsWith("~"))
            //    {
            //        var changedFile = (from file in Files
            //                           where file.Name.Equals(newFile.Name)
            //                           select file).FirstOrDefault();
            //        if (!newFile.Equals(changedFile))
            //        {
            //            Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
            //            this.Files.Remove(changedFile);
            //            this.Files.Add(newFile);
            //            File.Delete(Path.Combine(targetDirectory.FullName, changedFile.Name));
            //            TryCopyFile(newFile).Wait();
            //        }
            //    }
            //};
            //fileWatcher.Deleted += (s, e) =>
            //{
            //    Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
            //    Events.Insert(0, sourceDirectory.Root.ToString());
            //    var deletedFile = (from file in Files
            //                       where file.Name.Equals(e.Name)
            //                       select file).FirstOrDefault();
            //    this.Files.Remove(deletedFile);
            //    while (targetDirectory.GetFiles(deletedFile.Name).IsReadOnly) { }
            //    File.Delete(Path.Combine(targetDirectory.FullName, deletedFile.Name));
            //};
            //fileWatcher.Created += (s, e) =>
            //{
            //    this.Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
            //    var newFile = new FileInfo(e.FullPath);
            //    this.Files.Add(newFile);
            //    var complete = false;
            //    while (!complete)
            //    {
            //        try
            //        {
            //            TryCopyFile(newFile).Wait();
            //            complete = true;
            //        }
            //        catch { }
            //    }
            //};
            //fileWatcher.Renamed += async (s, e) =>
            //{
            //    var newFile = new FileInfo(e.FullPath);
            //    this.Events.Insert(0, e.OldFullPath); Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
            //    var updatedFile = (from file in Files
            //                       where file.Name.Equals(e.OldName)
            //                       select file).FirstOrDefault();
            //    this.Files.Remove(updatedFile);
            //    this.Files.Add(newFile);
            //    System.GC.Collect();
            //    System.GC.WaitForPendingFinalizers();
            //    File.Delete(Path.Combine(targetDirectory.FullName, updatedFile.Name));
            //    await TryCopyFile(newFile);
            //};

            fileWatcher.EnableRaisingEvents = true;
        }

        public void OpenFile(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }

        private void LoadAllFiles()
        {
            this.Files.Clear();
            var targetFiles = targetDirectory.GetFiles();
            foreach (var file in this.sourceDirectory.EnumerateFiles())
            {
                this.Files.Add(file);
                if(!targetFiles.Contains(file))
                {
                    var complete = false;
                    while (!complete)
                    {
                        try
                        {
                            TryCopyFile(file).Wait();
                            complete = true;
                        }
                        catch (Exception e)
                        { }
                    }
                }
            }
        }

        async private Task<FileInfo> TryCopyFile(FileInfo newFile)
        {
            return await Task.Factory.StartNew(() =>
                newFile.CopyTo(Path.Combine(targetDirectory.FullName, newFile.Name), true)
            );
        }
    }
}
