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
        public int numberOfFiles { get; set; }

        public DownloadManager (string downloadPath)
        {
            var directory = new DirectoryInfo(downloadPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            this.downloadPath = directory.FullName;
        }

        public async Task<string[]> GetAllFilesAsync ()
        {
            return await Task.Run( () => Directory.GetFiles(downloadPath));
        }
    }
}
