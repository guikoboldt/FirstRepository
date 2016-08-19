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
        private string _displayMessage;
        public string DisplayMessage
        {
            get { return this._displayMessage; }
            set { this._displayMessage = value; OnPropertyChanged(nameof(DisplayMessage)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
