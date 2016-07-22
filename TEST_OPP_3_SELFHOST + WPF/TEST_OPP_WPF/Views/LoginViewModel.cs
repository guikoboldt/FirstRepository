using EasyWpfLoginNavigateExample.Helpers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TEST_OPP_WPF.Views
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public static event EventHandler OnRequestClose;

        public string ButtonContent
        {
            get { return "Login"; }
        }

        public string LabelContentName
        {
            get { return "Username: "; }
        }

        public string LabelContentPass
        {
            get { return "Password: "; }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(CheckLogin);
            }
        }

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        private async void CheckLogin(object obj)
        {
            var _password = (obj as PasswordBox).Password;
            try
            {
                await Entities.GlobalInformations.ExecuteUri("api/User/", new Entities.User(username: _userName, password: _password));
            
            //Entities.GlobalInformations.Hub.On("Login", result => validCredentials = result);

            //Entities.GlobalInformations.Hub.Invoke("Login", _userName, _password).Wait();

            //if (Entities.GlobalInformations.ServerResponse.IsSuccessStatusCode)
            //{
                if (Entities.GlobalInformations.ServerResponse.IsSuccessStatusCode)
                {
                    Entities.GlobalInformations.nomeUsuario = Entities.GlobalInformations.ServerResponse.Content.ToString();
                    OnRequestClose(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Invalid Credentials");
                }
            }
            catch
            {
                MessageBox.Show("Bad Connection! Check your network");
            }
            // }
            //else
            //{
            //    MessageBox.Show("Bad Connection! Check your network");
            //}

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
