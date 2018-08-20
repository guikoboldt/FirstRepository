using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyBackupFile
{
    public class CopyFile
    {
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        static void Main(string[] args)
        {
            var originalPath = appSettings.Get("OriginalServerSourcePath");
            var fileTypeForMonitoring = appSettings.Get("FileTypeForMonitoring");
            var destinationServerPath = appSettings.Get("DestinationServerTargetPath");
            var destinationServerUser = appSettings.Get("DestinationServerUser");
            var destinationServerDomain = appSettings.Get("DestinationServerDomain");
            var destinationServerPassword = appSettings.Get("DestinationServerPassword");

            var manager = new Entities.FileManager(originalPath, fileTypeForMonitoring,
                destinationServerPath, destinationServerUser, destinationServerDomain, 
                destinationServerPassword);

            Console.Read();
        }
    }
}
