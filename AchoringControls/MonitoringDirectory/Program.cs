﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var dm = new Entities.WindowsFileManager(@"C:\Download", @"C:\Download_copy\Test");
            //var dm_final = new Entities.FileManager(@"C:\Download_copy", @"C:\Download_copy2");
            var ftp = new Entities.FTPFileManager("FTP://179.184.113.34/", "opp", "sindus1*");
            foreach (var file in ftp.Files)
            {
                Console.WriteLine("Name: {0}    Location: {1}  ", file.Name, file.Location);
            }

            Console.Read();
        }
    }
}
