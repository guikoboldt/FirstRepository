using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_OPP_WPF.Entities
{
    public static class GlobalInformations
    {
        public static string nomeUsuario { get; set; }
        public static HubConnection connection { get; set; }
        public static IHubProxy Hub { get; set; }

        public static async Task<bool> Connection()
        {
            var status = false;
            connection = new HubConnection("http://localhost:9000/");
            Hub = connection.CreateHubProxy("HubClass");
            await Task.Run(() =>
            {
                try
                {
                    connection.Start().Wait(TimeSpan.FromMilliseconds(4000));
                }
                catch (AggregateException e)
                { }
            });
            if (connection.State.Equals(ConnectionState.Connected))
            {
                status = true;
            }
            return status;
        }
    }
}
