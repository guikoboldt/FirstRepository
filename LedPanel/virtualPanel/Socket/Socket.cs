using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace virtualPanel.Socket
{
    public class Socket
    {
        public TcpListener server { get; private set; } = new TcpListener(2034);
        public Socket()
        {
            server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            server.Start();
            server.Server.Listen(5);
        }
        public async Task<string> GetMessageFromServer()
        {
            byte[] buffer = new byte[1024];

            var socket = await server.AcceptSocketAsync();
            var length = socket.Receive(buffer);

            var dataLength = ((ushort)buffer[11]) << 8;
            dataLength = dataLength | (ushort)buffer[12];

            var data = ASCIIEncoding.ASCII.GetString(buffer.Skip(13).Take(dataLength).ToArray());
            var dataByte = buffer.Skip(13).Take(dataLength);

            var pakect = string.Format(
                    @"
                        SOH:      {0} 
                        STX:      {1}
                        CLAS. ORIG.:    {2}
                        GROUP ORIG.:    {3}
                        ID ORIG.:    {4}
                        CLAS. DEST.: {5}    
                        GROUP DEST.: {6}
                        ID DEST.:    {7}
                        CMD:      {8} 
                        NFR:      {9} 
                        NFRAME:   {10}
                        TAM_DATA: {11}
                        DATA:     {12}
                        ", buffer.Cast<object>().Take(11).Concat(new object[] { dataLength }).Concat(new[] { data }).ToArray());

            var message = "";

            foreach (var item in dataByte)
            {
                switch (item)
                {
                    case 0xAA:
                    case 0x40:
                    case 0x0A:
                    case 0x04:
                    case 0x01:
                    case 0x11:
                    case 0x16:
                    case 0x13://command
                        {
                            break;
                        }
                    case 0x20: //space
                        {
                            message += " ";
                            break;
                        }
                    case 0x24: //scroll down
                        {
                            message += "\r\n";
                            break;
                        }
                    default: //default message
                        {
                            message += ASCIIEncoding.ASCII.GetString(new byte[] { item });
                            break;
                        }
                }
            }

            return await Task.Factory.StartNew(() => message);
        }
    }
}
