using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TEST_OPP_3_SELFHOST___WPF.Database;

namespace TEST_OPP_3_SELFHOST___WPF.Server_Features
{
   public class LoginController : ApiController
    {
        public IHttpActionResult GetLogin(string[] typedUser)
        {
            return Ok(CheckUser(typedUser[0], typedUser[1]));
        }

        private user CheckUser(string username, string passwordOrEmail)
        {
            user checkedUser = null;
            if (username != null && username != "" && passwordOrEmail != null && passwordOrEmail != "")
            {
                using (var db = new Database.TEST_OPPEntities())
                {
                    checkedUser = (from a in db.userSet
                                   where a.username.Equals(username) && (a.password.Equals(passwordOrEmail) || a.email.Equals(passwordOrEmail))
                                   select a).FirstOrDefault();

                }
            }
            return checkedUser;
        }
    }
}
