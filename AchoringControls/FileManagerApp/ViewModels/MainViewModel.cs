using FileManagerApp.Helpers;
using FileManagerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManagerApp.Models
{
    public class MainViewModel : IBaseViewModel
    {
        public string DowloadManager
        { get { return "Dowload Manager"; } }

        public string Name
        { get { return "Main Window"; } }
    }
}
