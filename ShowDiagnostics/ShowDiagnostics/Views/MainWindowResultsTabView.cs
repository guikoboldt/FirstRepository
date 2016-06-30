using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowDiagnostics.Views
{
    class MainWindowResultsTabView : INotifyPropertyChanged
    {
        private double _cpuAverage;
        public double CpuAverage
        {
            get { return _cpuAverage; }
            set { _cpuAverage = value; OnPropertyChanged("CpuAverage"); }
        }

        private double _cpuMaximun;
        public double CpuMaximun
        {
            get { return _cpuMaximun; }
            set { _cpuMaximun = value; OnPropertyChanged("CpuMaximun"); }
        }

        private double _cpuMinimun;
        public double CpuMinimun
        {
            get { return _cpuMinimun; }
            set { _cpuMinimun = value; OnPropertyChanged("CpuMinimun"); }
        }

        private double _ramAverage;
        public double RamAverage
        {
            get { return _ramAverage; }
            set { _ramAverage = value; OnPropertyChanged("RamAverage"); }
        }

        private double _ramMaximun;
        public double RamMaximun
        {
            get { return _ramMaximun; }
            set { _ramMaximun = value; OnPropertyChanged("RamMaximun"); }
        }

        private double _ramMinimun;
        public double RamMinimun
        {
            get { return _ramMinimun; }
            set { _ramMinimun = value; OnPropertyChanged("RamMinimun"); }
        }

        private double _diskAverage;
        public double DiskAverage
        {
            get { return _diskAverage; }
            set { _diskAverage = value; OnPropertyChanged("DiskAverage"); }
        }

        private double _diskMaximun;
        public double DiskMaximun
        {
            get { return _diskMaximun; }
            set { _diskMaximun = value; OnPropertyChanged("DiskMaximun"); }
        }

        private double _diskMinimun;
        public double DiskMinimun
        {
            get { return _diskMinimun; }
            set { _diskMinimun = value; OnPropertyChanged("DiskMinimun"); }
        }

        private double _networkSentAverage;
        public double NetworkSentAverage
        {
            get { return _networkSentAverage; }
            set { _networkSentAverage = value; OnPropertyChanged("NetworkSentAverage"); }
        }

        private double _networkSentMaximun;
        public double NetworkSentMaximun
        {
            get { return _networkSentMaximun; }
            set { _networkSentMaximun = value; OnPropertyChanged("NetworkSentMaximun"); }
        }

        private double _networkSentMinimun;
        public double NetworkSentMinimun
        {
            get { return _networkSentMinimun; }
            set { _networkSentMinimun = value; OnPropertyChanged("NetworkSentMinimun"); }
        }

        private double _networkReceivedAverage;
        public double NetworkReceivedAverage
        {
            get { return _networkReceivedAverage; }
            set { _networkReceivedAverage = value; OnPropertyChanged("NetworkReceivedAverage"); }
        }

        private double _networkReceivedMaximun;
        public double NetworkReceivedMaximun
        {
            get { return _networkReceivedMaximun; }
            set { _networkReceivedMaximun = value; OnPropertyChanged("NetworkReceivedMaximun"); }
        }

        private double _networkReceivedMinimun;
        public double NetworkReceivedMinimun
        {
            get { return _networkSentMinimun; }
            set { _networkSentMinimun = value; OnPropertyChanged("NetworkSentMinimun"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
