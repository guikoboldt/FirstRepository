using FileManagerApp.Exceptions;
using System;
using System.Collections.Generic;
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

        public DownloadManager (string downloadPath)
        {
            var directory = new DirectoryInfo(downloadPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            this.downloadPath = directory.FullName;
        }

        public FileInfo[] GetAllFiles()
        {
            var files = GetAllFilesAsync().Result;
            if (files.Length.Equals(0))
                throw new NoFilesFoundException();
            return files;

        }

        private async Task<FileInfo[]> GetAllFilesAsync ()
        {
            return await Task.Run( () => new DirectoryInfo(downloadPath).GetFiles());
        }
    }
}
