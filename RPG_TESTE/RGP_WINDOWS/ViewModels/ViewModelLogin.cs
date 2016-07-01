using EasyWpfLoginNavigateExample.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RGP_WINDOWS.ViewModels
{
    class ViewModelLogin : INotifyPropertyChanged
    { 
        public ViewModelLogin()
        {
            LoginCommand = new RelayCommand(CheckLogin);
        }

        public ICommand LoginCommand { get; set; }

        public string ButtonContent
        {
            get { return "Login"; }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged("Login"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged (this, new PropertyChangedEventArgs(propertyName));
        }

        private void CheckLogin (object obj)
        {
            var checkImputs = CheckImputsValues();
            if (checkImputs)
            {
                MessageBox.Show("User: " + _login + " -- Password: " + _password + " !!");
            }
            else
            {
                LoginError("Blank user or password");
            }
        }

        private bool CheckImputsValues ()
        {
            bool status = false;
            if (_login != "" && _password != "")
            {
                status = true;
            }
            return status;
        }

        private void LoginError (string error)
        {
            MessageBox.Show(error);
        }
    }
}
