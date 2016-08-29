using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace TEST_OPP_WPF.Views
{
    class entryUserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value;  OnPropertyChanged("User"); }
        }

        public string LabelName
        { get { return "User Name"; } }

        private string _password { get; set; }
        private string _userName { get; set; }
        public string UserName
        {
            get { return this._userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        private void EntryUser(object obj)
        {
            using (var db = new TEST_OPP_WPF.Views.newTestEntities() )
            {
                var user = (from line in db.Users
                            select line);
                foreach (var item in user)
                {
                    Users.Add(item);
                }
            }
            
        }

        public ICommand EntryUserCommand
        {
            get { return new Helpers.RelayCommand(EntryUser); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
