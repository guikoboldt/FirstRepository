using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerApp.Globals
{
    public class GlobalInformations
    {
        public static string baseDirectory { get; } = AppDomain.CurrentDomain.BaseDirectory;
        public static string defultDownloadPath { get; } = Path.Combine(baseDirectory , System.Configuration.ConfigurationManager.AppSettings["defaultDownloadPath"]);
    }
}
