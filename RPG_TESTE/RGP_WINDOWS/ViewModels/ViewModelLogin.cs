using EasyWpfLoginNavigateExample.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RGP_WINDOWS.ViewModels
{
    class ViewModelLogin : INotifyPropertyChanged
    {
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(CheckLogin);
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CheckLogin(object obj)
        {
           var  _password = (obj as PasswordBox).Password;
            using (var db = new Database.RPG_DATABASEEntities())
            {
                var query = (from user in db.USER
                             where user.NickName == _login
                             where user.Password == _password
                             select user).FirstOrDefault();

                if (query == null)
                {
                    Messages("Invalid user or password");
                }
                else
                {
                    // go to main windown
                    Messages("First Name: " + query.FirstName  + " -- Email: " + query.Email);
                }
            }
        }

        private void Messages(string message)
        {
            MessageBox.Show(message);
        }
    }
}
