﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public static async Task NotifyAllMembers(string eventName)
        {
            Hub.On(eventName, (isAvailable) => MessageBox.Show(isAvailable));
            await Hub.Invoke(eventName);
        }

        public static void StartServerConnection()
        {
            ServerConnection = new HttpClient();
            ServerConnection.BaseAddress = new Uri("http://localhost:9000");
            ServerConnection.DefaultRequestHeaders.Accept.Clear();
            ServerConnection.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task ExecuteUri (string address, User parameter)
        {
            ServerResponse = await ServerConnection.PostAsJsonAsync<User>(address, parameter);
        }
    }
}
