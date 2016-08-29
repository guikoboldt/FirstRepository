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

namespace FileManagerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IBaseViewModel
    {
        public MainViewModel()
        {
            dM = new DownloadManager(Globals.GlobalInformations.defultDownloadPath);
            LoadFiles();
        }

        private ObservableCollection<FileInfo> _files;
        public ObservableCollection<FileInfo> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged("Files"); }
        }

        private ObservableCollection<string> _events;
        public ObservableCollection<string> Events
        {
            get { return _events; }
            set { _events = value; OnPropertyChanged("Events"); }
        }

        private DownloadManager dM;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string UpdateFiles
        { get { return "Update Files"; } }
        public string DeleteFile
        { get { return "Delete"; } }

        public string Name
        { get { return "Main Window"; } }

        //public IList fileList{ get; private set; }
        private void LookForFiles(object obj)
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            this.Files = dM.Files;
            this.Events = dM.Events;
        }

        private void OpenCommand(object obj)
        {
            try
            {
                if (obj is FileInfo)
                {
                    dM.OpenFile((obj as FileInfo).FullName);
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to delete this file");
            }
        }

        private void DeleteCommand(object obj)
        {
            try
            {
                if (obj is FileInfo)
                {
                    File.Delete((obj as FileInfo).FullName);
                    this.Files = dM.Files;
                    this.Events = dM.Events;
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to delete this file");
            }
        }

        public ICommand UpdateFilesCommand
        { get { return new RelayCommand(LookForFiles); } }

        public ICommand OpenFileCommand
        { get { return new RelayCommand(OpenCommand); } }

        public ICommand DeleteFileCommand
        { get { return new RelayCommand(DeleteCommand); } }
    }
}
