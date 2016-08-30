using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Interfaces
{
    interface IFileProvider
    {
        FileInfo[] Files(string path);
    }
}
