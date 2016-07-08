using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TEST_OPP_3_SELFHOST___WPF.Server_Features
{
   public class TestController : ApiController
    {

        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("Server is communicanting"),
            };
        }
    }
}
