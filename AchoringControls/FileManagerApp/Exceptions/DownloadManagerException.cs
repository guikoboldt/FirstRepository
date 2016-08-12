using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerApp.Exceptions
{
    public class DownloadManagerException : Exception
    {
        public DownloadManagerException()
            : base("Oh no! something is wrong. Try to reinstall the application!") {}
        public DownloadManagerException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }

    public class NoFilesFoundException : DownloadManagerException
    {
        public NoFilesFoundException()
            : base("No files availables!") { }
    }
}
