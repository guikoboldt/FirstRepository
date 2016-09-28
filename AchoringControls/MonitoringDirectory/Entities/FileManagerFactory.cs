using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class FileManagerFactory
    {
        static public Interfaces.IFileProvider LoadFileManager (string path, string user = "", string password = "")
        {
            if (path.Contains("FTP"))
                return new FTPFileManager(path, user, password);
            else
                return new WindowsFileManager(path, path);
        }

        static public Interfaces.IFileProvider LoadFileManager(string path, string targetPath = @"C:\Temp\")
        {
            return new WindowsFileManager(path, path);
        }

    }
}
