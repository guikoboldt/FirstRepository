using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virtualPanel.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public string _displayMessage;
        public string DisplayMessage
        {
            get { return _displayMessage; }
            set { _displayMessage = value; OnPropertyChanged("DisplayMessage"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
