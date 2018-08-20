using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Previsao.Temp.Lab.ViewModels
{
    public class BuscaPrevisoesViewModel : INotifyPropertyChanged
    {
        private string _localizacao { get; set; }
        private ObservableCollection<Models.Previsoes> _previsoes { get; set; }
        public BuscaPrevisoesViewModel(IEnumerable<Models.Previsoes> previsoes, string city)
        {
            Localizacao = city;
            Previsoes = new ObservableCollection<Models.Previsoes>(previsoes);
        }

        public string Localizacao
        {
            get { return this._localizacao; }
            set { this._localizacao = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Models.Previsoes> Previsoes
        {
            get { return this._previsoes; }
            set { this._previsoes = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged ([CallerMemberName] string propertyChanged = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }
    }
}
