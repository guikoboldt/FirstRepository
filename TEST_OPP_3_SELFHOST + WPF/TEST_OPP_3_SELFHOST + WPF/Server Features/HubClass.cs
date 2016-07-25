using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEST_OPP_3_SELFHOST___WPF.Database;

namespace TEST_OPP_3_SELFHOST___WPF.Server_Features
{
    public class HubClass : Hub
    {
        public void NotifyOnLogon (string username)
        {
            Clients.Others.NotifyAllMembers("{0} is now online", username);
        }
    }
}
