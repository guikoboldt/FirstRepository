using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new WebRequestHandler();
            //handler.ServerCertificateValidationCallback = (sender, /*X509Certificate*/ certificate, /*X509Chain*/ chain, /*SslPolicyErrors*/ sslPolicyErrors) =>
            //{
            //    sender("Sender"); certificate("Certificate"); chain("Chain"); sslPolicyErrors();
            //};
            var options = new StartOptions();

            options.Urls.Add("http://+:9000/");
            options.Urls.Add("https://+:4443/");

            var cert = new X509Certificate2();

            using (WebApp.Start<Startup>(options))
            {
                Console.Read();
            }
        }

        public class Startup
        {
            // This code configures Web API. The Startup class is specified as a type
            // parameter in the WebApp.Start method.
            public void Configuration(IAppBuilder appBuilder)
            {
                // Configure Web API for self-host. 
                HttpConfiguration config = new HttpConfiguration();

                config.Routes.MapHttpRoute(
                    name: "ActionApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                config.MapHttpAttributeRoutes();

                appBuilder.UseWebApi(config);
            }
        }
    }
}
