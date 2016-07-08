using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TEST_OPP_3_SELFHOST___WPF
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new StartOptions();
            options.Urls.Add("http://+:9000");

            using (WebApp.Start<Server_Features.Startup>(options))
            {
                Console.Write("Server running");
                Console.Read();
            }
        }
    }
}
