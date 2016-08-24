using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace LedPanelBDD.Initializer
{
    [Binding]
    public sealed class SocketInitializer
    {
        protected PanelContext context;
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public SocketInitializer(PanelContext context)
        {
            this.context = context;
        }

        [BeforeScenario("requires_socket")]
        public void BeforeScenario()
        {
            this.context.Socket.server.Start();
        }

        [AfterScenario("requires_socket")]
        public void AfterScenario()
        {
            this.context.Socket.server.Stop();
        }
    }
}
