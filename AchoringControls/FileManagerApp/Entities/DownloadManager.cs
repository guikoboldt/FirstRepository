using FileManagerApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerApp.Entities
{
    public class DownloadManager
    {
        public DirectoryInfo directory { get; set; }

        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        private FileSystemWatcher directoryWatcher = new FileSystemWatcher();
        public ObservableCollection<FileInfo> Files { get; private set; } = new ObservableCollection<FileInfo>();
        public ObservableCollection<string> Events { get; private set; } = new ObservableCollection<string>();

        public DownloadManager(string downloadPath)
        {
            directory = new DirectoryInfo(downloadPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            LoadAllFiles();
            ConfigureFileWatcher();
            ConfigureDirectoryWatcher();
        }

        private void ConfigureDirectoryWatcher()
        {
            directoryWatcher.Path = directory.Root.ToString();
            directoryWatcher.NotifyFilter = NotifyFilters.DirectoryName;
            directoryWatcher.IncludeSubdirectories = true;

            directoryWatcher.Renamed += (s, e) =>
            {
                if (e.OldFullPath.Equals(directory.FullName))
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Events.Insert(0, e.OldFullPath);
                        Events.Insert(0, e.FullPath);
                        directory = new DirectoryInfo(e.FullPath);
                        fileWatcher.Path = this.directory.FullName;
                        LoadAllFiles();
                    });
                }
            };

            directoryWatcher.EnableRaisingEvents = true;
        }

        public void ConfigureFileWatcher()
        {
            fileWatcher.Path = this.directory.FullName;
            fileWatcher.Filter = "*";

            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;


            fileWatcher.Changed += (s, e) =>
            {
                var newFile = new FileInfo(e.FullPath);
                var chengedFile = (from file in Files
                                   where file.Name.Equals(newFile.Name)
                                   select file).FirstOrDefault();
                if(!newFile.Equals(chengedFile))
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
                        Files.Remove(chengedFile);
                        Files.Add(newFile);
                    });
                }
            };
            fileWatcher.Deleted += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
                    Events.Insert(0, directory.Root.ToString());
                    var deletedFile = (from file in Files
                                      where file.Name.Equals(e.Name)
                                      select file).FirstOrDefault();
                    Files.Remove(deletedFile);
                });
            };
            fileWatcher.Created += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
                    var newFile = new FileInfo(e.FullPath);
                    this.Files.Add(newFile);
                });
            };
            fileWatcher.Renamed += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.Events.Insert(0, e.OldFullPath); Events.Insert(0, e.FullPath); Events.Insert(0, e.ChangeType.ToString());
                    var updatedFile = (from file in Files
                                       where file.Name.Equals(e.OldName)
                                       select file).FirstOrDefault();
                    this.Files.Remove(updatedFile);
                    this.Files.Add(new FileInfo(e.FullPath));
                });
            };

            fileWatcher.EnableRaisingEvents = true;
        }

        private void LoadAllFiles()
        {
            this.Files.Clear();
            foreach (var file in this.directory.EnumerateFiles())
            {
                this.Files.Add(file);
            }
        }
    }
}
