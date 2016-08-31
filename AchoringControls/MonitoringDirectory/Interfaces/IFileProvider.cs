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
        ICollection<Entities.File> Files { get; }

        void DeleteFile(Entities.File file);

        void OpenFile(Entities.File file);

        Task CopyFileTo(Entities.File file, string destination);

    }
}
