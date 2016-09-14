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
    public class MainViewModel : MonitoringDirectory.Entities.FileManager, INotifyPropertyChanged
    {
        //public ObservableCollection<MonitoringDirectory.Entities.File> _Files { get; set; } = new ObservableCollection<MonitoringDirectory.Entities.File> ();
        public MainViewModel()
            : base(@"C:\Download\")
        {
            base.Files = base.GetFiles(@"C:\Download\");
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
            try
            {
                if (obj is MonitoringDirectory.Entities.File)
                {
                    base.OpenFile(obj as MonitoringDirectory.Entities.File);
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
            try
            {
                if (obj is MonitoringDirectory.Entities.File)
                {
                    base.DeleteFile(obj as MonitoringDirectory.Entities.File);
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to delete this file");
            }
        }

        public override void OnDeleted(MonitoringDirectory.Entities.File file)
        {
            var deleted = base.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
            if (deleted != null)
            {
                App.Current.Dispatcher.Invoke(() => 
                base.Files.Remove(deleted));
            }
        }

        public override void OnChanged(MonitoringDirectory.Entities.File file)
        {
            var changed = base.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
            if (changed != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                base.Files.Remove(changed));
                base.DeleteFile(changed);
            }
            file.LastChange = DateTime.Now;
            file.Size = changed.Size;
            App.Current.Dispatcher.Invoke(() =>
               base.Files.Add(file));
            base.CopyFileTo(file, @"C:\Download_copy\");
        }

        public override void OnRenamed(MonitoringDirectory.Entities.File oldFile, MonitoringDirectory.Entities.File file)
        {
            var fileToSearch = oldFile.Name;
            if (oldFile.Name.StartsWith("~"))
                fileToSearch = file.Name;
            var renamed = base.Files.FirstOrDefault(_ => _.Name.Equals(fileToSearch));
            if (renamed != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                base.Files.Remove(renamed));
                base.DeleteFile(renamed);
            }
            file.LastChange = DateTime.Now;
            file.Size = renamed.Size;
            App.Current.Dispatcher.Invoke(() =>
               base.Files.Add(file));
            base.CopyFileTo(file, @"C:\Download_copy\");
        }

        public ICommand OpenFileCommand
        { get { return new RelayCommand(OpenCommand); } }

        public ICommand DeleteFileCommand
        { get { return new RelayCommand(DeleteCommand); } }
    }
}
