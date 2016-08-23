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
            //server.Start();
            //System.Net.Sockets.Socket socket;
            //System.Net.EndPoint endPointClient;
            byte[] buffer = new byte[1024];
            //int length;
            //byte[] dataSize = new byte[2];
            //while (true)
            //{
            //if (!server.Server.Connected)
            //{
                var socket = await server.AcceptSocketAsync();
                var length = socket.Receive(buffer);
            //}
            //else
            //{
            //    length = server.Server.Receive(buffer);
            //}
            //Array.Copy(buffer, 11, dataSize, 0, 2);

            var dataLength = ((ushort)buffer[11]) << 8;
            dataLength = dataLength | (ushort)buffer[12];

            var data = ASCIIEncoding.ASCII.GetString(buffer.Skip(13).Take(dataLength).ToArray());

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

            return await Task.Factory.StartNew(() => pakect.Substring(pakect.LastIndexOf("DATA") + 5));
            // }
        }
    }
}
