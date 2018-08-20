using Previsao.Temp.Models;
using Previsao.Temp.Lab.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Previsao.Temp.Lab.ViewModels
{
    public class BuscaCidadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _localizacao { get; set; }
        private Cidade _selectedCity { get; set; }
        private ObservableCollection<Cidade> _cidades { get; set; }
        private Cptec _webServer { get; }
        private ICommand _buscaCidades;
        private ICommand _selectCommand;

        public BuscaCidadesViewModel()
        {
            this._localizacao = "Porto Alegre";
            this.Cidades = new ObservableCollection<Cidade>();
            this._webServer = new Cptec();
        }

        public string Localizacao
        {
            get { return this._localizacao; }
            set { this._localizacao = value; OnPropertyChanged(); }
        }

        public Cidade SelectedCity
        {
            get { return this._selectedCity; }
            set { this._selectedCity = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Cidade> Cidades
        {
            get { return this._cidades; }
            set { this._cidades = value; OnPropertyChanged(); }
        }

        public ICommand BuscarCidadesCommand =>
            _buscaCidades ?? (_buscaCidades = new Command(
                async () => await ExecutarBuscarCidadesCommand()));

        public ICommand SelectCommand =>
            _selectCommand ?? (_selectCommand = new Command(
                async () => await ExecuteGetWheterCommand()));

        async private Task ExecuteGetWheterCommand()
        {
            var dados = await _webServer.GetWheter(SelectedCity.Id);
            var previsoesViewModel = new ViewModels.BuscaPrevisoesViewModel(dados, SelectedCity.Nome);
            var previsoesView = new Views.BuscaPrevisoesView();
            previsoesView.BindingContext = previsoesViewModel;
            await App.Current.MainPage.Navigation.PushAsync(previsoesView);
        }
        async private Task ExecutarBuscarCidadesCommand()
        {
            var dados = await _webServer.GetCidades(Localizacao);
            Cidades = new ObservableCollection<Cidade> (dados);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
