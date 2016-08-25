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
        public string downloadPath { get; set; }
        public FileSystemWatcher fileWatcher = new FileSystemWatcher();
        public ObservableCollection<FileInfo> files { get; private set; }
        public ObservableCollection<string> events { get; private set; }

        public DownloadManager(string downloadPath)
        {
            var directory = new DirectoryInfo(downloadPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            this.downloadPath = directory.FullName;
            events = new ObservableCollection<string>();
            LoadAllFiles();
            watcherConfigure();
        }

        public void watcherConfigure()
        {
            fileWatcher.Path = this.downloadPath;
            fileWatcher.Filter = "*";

            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;


            fileWatcher.Changed += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString());
                });
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
                { events.Insert(0, e.OldFullPath); events.Insert(0, e.FullPath); events.Insert(0, e.ChangeType.ToString()); });
            };

            fileWatcher.EnableRaisingEvents = true;
        }

        private void LoadAllFiles()
        {
            files = new ObservableCollection<FileInfo>(new DirectoryInfo(downloadPath).GetFiles());
        }
    }
}
