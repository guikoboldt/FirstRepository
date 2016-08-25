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

        public string updateFiles
        { get { return "Update Files"; } }
        public string deleteFile
        { get { return "Delete"; } }

        public string name
        { get { return "Main Window"; } }

        //public IList fileList{ get; private set; }
        public void loadFiles(object obj)
        {
            this.Files = dM.files;
            this.Events = dM.events;
        }

        public void deleteCommand(object obj)
        {
            try
            {
                if (obj is FileInfo)
                {
                    File.Delete((obj as FileInfo).FullName);
                    this.Files = dM.files;
                    this.Events = dM.events;
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to delete this file");
            }
        }
        public ICommand updateFilesCommand
        { get { return new RelayCommand(loadFiles); } }

        public ICommand deleteFileCommand
        { get { return new RelayCommand(deleteCommand); } }
    }
}
