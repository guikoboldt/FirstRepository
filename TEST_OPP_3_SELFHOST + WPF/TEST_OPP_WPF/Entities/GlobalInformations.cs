using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TEST_OPP_WPF.Entities
{
    public static class GlobalInformations
    {
        public static string nomeUsuario { get; set; }
        public static HubConnection connection { get; set; }
        public static IHubProxy Hub { get; set; }
        public static HttpClient ServerConnection { get; set; }
        public static HttpResponseMessage ServerResponse { get; set; }

        public static async Task<bool> StartHubConnection()
        {
            connection = new HubConnection("http://localhost:9000/");
            Hub = connection.CreateHubProxy("HubClass");
                try
                {
                  await connection.Start();
                }
                catch (AggregateException e)
                { }
            return connection.State == ConnectionState.Connected;
        }

        public static async Task StartServerConnection()
        {
            ServerConnection = new HttpClient();
            ServerConnection.BaseAddress = new Uri("http://localhost:9000");
            ServerConnection.DefaultRequestHeaders.Accept.Clear();
            ServerConnection.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async void ExecuteUri (string parameter)
        {
            ServerResponse = await ServerConnection.GetAsync(parameter);
        }
    }
}
