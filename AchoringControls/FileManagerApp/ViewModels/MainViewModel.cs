using FileManagerApp.Helpers;
using FileManagerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManagerApp.ViewModels
{
    public class MainViewModel : IBaseViewModel
    {
        public string DownloadManager
        { get { return "Dowload Manager"; } }

        public string Name
        { get { return "Main Window"; } }
    }
}
