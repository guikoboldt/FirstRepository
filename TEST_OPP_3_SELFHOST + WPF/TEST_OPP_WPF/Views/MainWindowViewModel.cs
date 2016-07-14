using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_OPP_WPF.Views
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public string LoggedLabelContent
        {
            get { return "Hello " + Entities.GlobalInformations.nomeUsuario; }
        }

        public string[] TreeMenuEntries
        {
            get { return new string[] { "Products", "Users"}; }
        }

        public string[] TreeMenuManagement
        {
            get { return new string[] { "Not Implemented yet" }; }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
