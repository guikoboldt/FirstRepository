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
        public FileSystemWatcher fileWatcher = new FileSystemWatcher();
        public ObservableCollection<FileInfo> files { get; private set; }
        public ObservableCollection<string> events { get; private set; }

        public DownloadManager(string downloadPath)
        {
            directory = new DirectoryInfo(downloadPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            events = new ObservableCollection<string>();
            LoadAllFiles();
            watcherConfigure();
        }

        public void watcherConfigure()
        {
            fileWatcher.Path = this.directory.FullName;
            fileWatcher.Filter = "*";

            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;


            fileWatcher.Changed += (s, e) =>
            {
                var newFile = new FileInfo(e.FullPath);
                var chengedFile = (from file in files
                                   where file.Name.Equals(newFile.Name)
                                   select file).FirstOrDefault();
                if(!newFile.Equals(chengedFile))
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString());
                        files.Remove(chengedFile);
                        files.Add(newFile);
                    });
                }
            };
            fileWatcher.Deleted += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString());
                    var deletedFile = (from file in files
                                      where file.Name.Equals(e.Name)
                                      select file).FirstOrDefault();
                    files.Remove(deletedFile);
                });
            };
            fileWatcher.Created += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString());
                    var newFile = new FileInfo(e.FullPath);
                    files.Add(newFile);
                });
            };
            fileWatcher.Renamed += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    events.Insert(0, e.OldFullPath); events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString());
                    var updatedFile = (from file in files
                                       where file.Name.Equals(e.OldName)
                                       select file).FirstOrDefault();
                    files.Remove(updatedFile);
                    files.Add(new FileInfo(e.FullPath));
                });
            };

            fileWatcher.EnableRaisingEvents = true;
        }

        private void LoadAllFiles()
        {
            this.files = new ObservableCollection<FileInfo>(this.directory.GetFiles());
        }
    }
}
