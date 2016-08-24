using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedPanelBDD
{
    public class PanelContext
    {
        private Socket _socket { get; set; }
        private LedPanel.Entities.LedPanel _panel { get; set; }

        public PanelContext()
        {
            this._socket = new Socket();
            this._panel = new LedPanel.Entities.LedPanel("localhost", 2034);
        }

        public Socket Socket
        {
            get { return _socket = _socket ?? new Socket(); }
        }

        public LedPanel.Entities.LedPanel Panel
        {
            get { return _panel = _panel ?? new LedPanel.Entities.LedPanel("localhost"); }
        }
    }
}
