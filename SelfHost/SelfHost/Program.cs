using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace SelfHost
{
    public class HostController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent("Hello HTTP")
            };
        }
    }

    public class SelfHostHTTP
    {
        static void Main(string[] args)
        {
            var config = new MyHttpsSelfHostConfiguration("https://localhost:4443");
            var httpConfig = new HttpSelfHostConfiguration("http://localhost:8080");
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
            httpConfig.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
            var server = new HttpSelfHostServer(config);
            var httpServer = new HttpSelfHostServer(httpConfig);
            httpServer.OpenAsync().Wait();
            server.OpenAsync().Wait();

            Console.WriteLine("Server is opened");
            Console.ReadKey();

        }
    }

    class MyHttpsSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public MyHttpsSelfHostConfiguration(string baseAddress) : base(baseAddress) { }
        public MyHttpsSelfHostConfiguration(Uri baseAddress) : base(baseAddress) { }
        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.Security.Mode = HttpBindingSecurityMode.Transport;
            return base.OnConfigureBinding(httpBinding);
        }
    }


    public class testController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent("Test HTTP")
            };
        }
    }
}
