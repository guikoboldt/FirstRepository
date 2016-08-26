using FileManagerApp.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerBDD
{
    public class DownloadManagerTestContext
    {
        private DownloadManager _downloadManager { get; set; }
        private ObservableCollection<FileInfo> _files { get; set; }

        public DownloadManagerTestContext()
        {
            this._downloadManager = new DownloadManager(FileManagerApp.Globals.GlobalInformations.defultDownloadPath);
            this._files = new ObservableCollection<FileInfo>();
        }
        public DownloadManager DownloadManager
        {
            get { return _downloadManager = _downloadManager ?? new DownloadManager(FileManagerApp.Globals.GlobalInformations.defultDownloadPath); }
            set { _downloadManager = value; }
        }

        public ObservableCollection<FileInfo> Files
        {
            get { return _files = _files ?? new ObservableCollection<FileInfo>(); }
            set { _files = value; }
        }
    }
}
