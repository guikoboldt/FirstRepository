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
        Entities.File[] Files(string path);

        void DeleteFile(Entities.File file);

        void OpenFile(Entities.File file);

        void CopyFileTo(Entities.File file, string destination);

        Entities.File CreateFile(string name, int size, string location);
    }
}
