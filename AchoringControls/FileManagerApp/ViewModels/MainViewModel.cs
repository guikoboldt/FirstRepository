using FileManagerApp.Entities;
using FileManagerApp.Helpers;
using FileManagerApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManagerApp.ViewModels
{
    public class MainViewModel : ObservableObject, IBaseViewModel
    {
        public MainViewModel()
        {
            dM = new DownloadManager(Globals.GlobalInformations.defultDownloadPath);
        }

        private IList<FileInfo> _files;
        public IList<FileInfo> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged(nameof(Files)); }
        }
        private DownloadManager dM;
        public string updateFiles
        { get { return "Update Files"; } }
        public string deleteFiles
        { get { return "Delete Files"; } }

        public string name
        { get { return "Main Window"; } }

        //public IList fileList{ get; private set; }
        public void LoadFiles(object obj)
        {
            _files = dM.GetAllFiles().ToList();
        }

        public ICommand updateFilesCommand
        { get { return new RelayCommand(LoadFiles); } }
    }
}
