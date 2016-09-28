using FileManagerApp.Entities;
using FileManagerApp.Helpers;
using FileManagerApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MonitoringDirectory.Entities;

namespace FileManagerApp.ViewModels
{
    public class MainViewModel :INotifyPropertyChanged
    {
        public MonitoringDirectory.Interfaces.IFileProvider Manager { get; set; }
        public ObservableCollection<MonitoringDirectory.Entities.File> Files { get; set; } = new ObservableCollection<MonitoringDirectory.Entities.File> ();
        public MainViewModel()
        {
            Manager = MonitoringDirectory.Entities.FileManagerFactory.LoadFileManager(@"FTP://179.184.113.34/test", "opp", "sindus1*");
            foreach (var file in Manager.Files)
            {
                this.Files.Add(file);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string OpenFiles
        { get { return "Open File"; } }
        public string DeleteFiles
        { get { return "Delete"; } }

        //public IList fileList{ get; private set; }

        private void OpenCommand(object obj)
        {
            var opendFile = obj as MonitoringDirectory.Entities.File;
            try
            {
                if (opendFile is MonitoringDirectory.Entities.File)
                {
                    Manager.OpenFile(opendFile);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("An error occured when trying to open this file");
            }
            catch (Exception) { }
        }

        private void DeleteCommand(object obj)
        {
            var deletedFile = obj as MonitoringDirectory.Entities.File;
            try
            {
                if (deletedFile is MonitoringDirectory.Entities.File)
                {
                    Manager.DeleteFile(deletedFile);
                    this.Files.Remove(deletedFile);
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to delete this file");
            }
        }

        private void CopyCommand (object obj)
        {
            var copiedFile = obj as MonitoringDirectory.Entities.File;
            try
            {
                if (copiedFile is MonitoringDirectory.Entities.File)
                {
                    Manager.CopyFileTo(copiedFile, @"C:\Download_copy\");
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to copy this file");
            }
        }

        //public override void OnDeleted(MonitoringDirectory.Entities.File file)
        //{
        //    var deleted = base.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
        //    if (deleted != null)
        //    {
        //        App.Current.Dispatcher.Invoke(() => 
        //        base.Files.Remove(deleted));
        //    }
        //}

        //public override void OnChanged(MonitoringDirectory.Entities.File file)
        //{
        //    var changed = base.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
        //    if (changed != null)
        //    {
        //        App.Current.Dispatcher.Invoke(() =>
        //        base.Files.Remove(changed));
        //        base.DeleteFile(changed);
        //    }
        //    file.LastChange = DateTime.Now;
        //    file.Size = changed.Size;
        //    App.Current.Dispatcher.Invoke(() =>
        //       base.Files.Add(file));
        //    base.CopyFileTo(file, @"C:\Download_copy\");
        //}

        //public override void OnRenamed(MonitoringDirectory.Entities.File oldFile, MonitoringDirectory.Entities.File file)
        //{
        //    var fileToSearch = oldFile.Name;
        //    if (oldFile.Name.StartsWith("~"))
        //        fileToSearch = file.Name;
        //    var renamed = base.Files.FirstOrDefault(_ => _.Name.Equals(fileToSearch));
        //    if (renamed != null)
        //    {
        //        App.Current.Dispatcher.Invoke(() =>
        //        base.Files.Remove(renamed));
        //        base.DeleteFile(renamed);
        //        file.Size = renamed.Size;
        //    }
        //    file.LastChange = DateTime.Now;
        //    App.Current.Dispatcher.Invoke(() =>
        //       base.Files.Add(file));
        //    base.CopyFileTo(file, @"C:\Download_copy\");
        //}

        public ICommand OpenFileCommand
        { get { return new RelayCommand(OpenCommand); } }

        public ICommand DeleteFileCommand
        { get { return new RelayCommand(DeleteCommand); } }

        public ICommand CopyFileCommand
        { get { return new RelayCommand(CopyCommand); } }
    }
}
