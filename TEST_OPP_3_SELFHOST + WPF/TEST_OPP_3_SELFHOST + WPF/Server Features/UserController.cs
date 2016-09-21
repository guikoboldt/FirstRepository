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
   public class UserController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(string.Format("ok: {0} server is running", DateTime.Now)),
            };
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Entities.User user)
        {
            var checkedUser = CheckUser(user);
            if (checkedUser == null)
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound,
                                                    Content = new StringContent("Invalid User") };
            }
            else
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK,
                                                    Content = new StringContent(checkedUser.username) };
            }
        }

        private user CheckUser(Entities.User user)
        {
            user checkedUser = null;
            if (user.username != null && user.username != "" && user.password != null && user.password != "")
            {
                using (var db = new Database.TEST_OPPEntities())
                {
                    checkedUser = (from a in db.user
                                   where a.username.Equals(user.username) && a.password.Equals(user.password)
                                   select a).FirstOrDefault();

                }
            }
            return checkedUser;
        }
    }
}
