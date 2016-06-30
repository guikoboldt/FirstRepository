using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowDiagnostics.Views
{
    public class ViewModel : INotifyPropertyChanged
    {
        private double _cpu;
        public double Cpu
        {
            get { return _cpu; }
            set { _cpu = value; OnPropertyChanged("Cpu"); } //value is a propertie that comes from somewhere
        }

        private double _disk;
        public double Disk
        {
            get { return _disk; }
            set { _disk = value; OnPropertyChanged("Disk"); }
        }

        private double _networkReceived;
        public double NetworkReceived
        {
            get { return _networkReceived; }
            set { _networkReceived = value; OnPropertyChanged("NetworkReceived"); }
        }

        private double _networkSent;
        public double NetworkSent
        {
            get { return _networkSent; }
            set
            {
                if (_networkSent != value)
                {
                    _networkSent = value;
                    OnPropertyChanged("NetworkSent");
                }
            }
        }

        private double _ram;
        public double Ram
        {
            get { return _ram; }
            set { _ram = value; OnPropertyChanged("Ram"); }
        }

        private DateTime _initialDate;
        public DateTime InitialDate
        {
            get { return _initialDate; }
            set { _initialDate = value; OnPropertyChanged("InitialDate"); }
        }

        private DateTime _finalDate;
        public DateTime FinalDate
        {
            get { return _finalDate; }
            set { _finalDate = value; OnPropertyChanged("FinalDate"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
