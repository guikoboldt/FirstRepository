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

        //public IList<FileInfo> files = new List<FileInfo>();
        private DownloadManager dM;
        public string updateFiles
        { get { return "Update Files"; } }
        public string deleteFiles
        { get { return "Delete Files"; } }

        public string name
        { get { return "Main Window"; } }

        public IList fileList{ get; private set; }
        public void LoadFiles(object obj)
        {
            var files = dM.GetAllFiles().ToList();
            fileList = (from file in files
                            select new { fileName = file.Name, lastModified = file.LastWriteTime, size = file.Length }).ToList();
        }

        public ICommand updateFilesCommand
        { get { return new RelayCommand(LoadFiles); } }
    }
}
