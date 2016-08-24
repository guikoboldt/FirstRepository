using FileManagerApp.Entities;
using FileManagerApp.Helpers;
using FileManagerApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManagerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IBaseViewModel
    {
        public MainViewModel()
        {
            dM = new DownloadManager(Globals.GlobalInformations.defultDownloadPath);
        }

        private FileInfo[] _files;
        public FileInfo[]  Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged("Files"); }
        }
        private DownloadManager dM;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string updateFiles
        { get { return "Update Files"; } }
        public string deleteFiles
        { get { return "Delete Files"; } }

        public string name
        { get { return "Main Window"; } }

        //public IList fileList{ get; private set; }
        public void LoadFiles(object obj)
        {
             _files =  dM.GetAllFilesAsync();
        }

        public ICommand updateFilesCommand
        { get { return new RelayCommand(LoadFiles); } }
    }
}
