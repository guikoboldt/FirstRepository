using AssemblyCompiler.Utilitys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCompiler.ViewModel
{
    class ViewModelMainWindow : INotifyPropertyChanged
    {
        public List<string> _codeArea;
        public List<string>  CodeArea
        {
            get { return _codeArea; }
            set { _codeArea = value; OnPropertyChanged("CodeArea"); }
        }

        public List<CompilerAssembler> _resultsArea;
        public List<CompilerAssembler> ResultsArea
        {
            get { return _resultsArea; }
            set { _resultsArea = value; OnPropertyChanged("ResultsArea"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
