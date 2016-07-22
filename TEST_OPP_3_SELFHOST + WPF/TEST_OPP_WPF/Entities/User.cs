using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_OPP_WPF.Entities
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string name { get; set; }

        public User (string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
