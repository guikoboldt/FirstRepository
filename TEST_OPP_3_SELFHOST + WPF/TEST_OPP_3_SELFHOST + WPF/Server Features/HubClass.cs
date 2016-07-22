using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEST_OPP_3_SELFHOST___WPF.Database;

namespace TEST_OPP_3_SELFHOST___WPF.Server_Features
{
    public class HubClass : Hub
    {

        public void Login(string username, string password)
        {
            string userInformations = null;
            var user = CheckUser(username, password);
            if (user != null)
                userInformations = user.name;
            Clients.Caller.Login(userInformations);
        }

        public void CreateUser(string username, string password, string email)
        {
            var checkedUser = CheckUser(username, email);
            if (checkedUser != null) //check if this user was create
            {
                Clients.Caller("This user was already create");
            }
            else
            { //if not, create a new one
                using (var db = new Database.TEST_OPPEntities())
                {
                    var newUser = new user();
                    newUser.username = username;
                    newUser.password = password;
                    newUser.email = email;

                    db.user.Add(newUser);
                    db.SaveChanges();
                }
            }

        }

        private user CheckUser(string username, string passwordOrEmail)
        {
            user checkedUser = null;
            if (username != null && username != "" && passwordOrEmail != null && passwordOrEmail != "")
            {
                using (var db = new Database.TEST_OPPEntities())
                {
                    checkedUser = (from a in db.user
                                           where a.username.Equals(username) && (a.password.Equals(passwordOrEmail) || a.email.Equals(passwordOrEmail))
                                           select a).FirstOrDefault();

                }
            }
            return checkedUser;
        }
    }
}
