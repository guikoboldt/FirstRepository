using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinSelfHost.Controllers
{
    public class TestController : ApiController
    {
        //[HttpGet]
        //public string Date ()
        //{
        //    return DateTime.Now.ToString();
        //}
        public HttpResponseMessage Get()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;

            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(string.Format("ok: {0} version: {1}", DateTime.Now, version)),
            };
        }

        public HttpResponseMessage Post([FromBody] object messageFromNode)
        {
            LedPanel.Entities.LedPanel.SendMessage("localhost", 2034, messageFromNode.ToString(), " ", LedPanel.Entities.LedPanel.MessageType.Normal_1L).Wait();
            return new HttpResponseMessage { Content = new StringContent("OK") };
        }
    }
}
