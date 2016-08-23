using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LedPanel.Entities
{
    public class LedPanel
    {
        public Socket socket { get; set; }
        private string ipAddress { get; set; }
        private int connectionPort { get; set; } //default panel's connection port: 2101;

        private const byte espace = 0x20;
        private const byte command = 0xAA;
        private const byte defaultFontSize = 0x11;
        private const byte bigFontSize = 0x16;
        private const byte mediumFontSize = 0x13;
        private const byte firstLine = 0x01;
        private const byte secondLine = 0x02;
        private const byte alignCenter = 0x04;
        private const byte speedSetter = 0x40;
        private const byte slow = 0x0A;
        private const byte scrollDown = 0x24;
        private const byte scrollUp = 0x23;
        private const byte initiateFrame = 0x01;
        private const byte initiateMessage = 0x02;
        private const byte endFrame = 0x03;
        private const byte computerCode = 0x50;
        private const byte computerGroup = 0x01;
        private const byte computerId = 0x01;
        private const byte panelCode = 0xAA;
        private const byte panelGroup = 0x01;
        private const byte panelId = 0x01;
        private const byte quickMessage = 0x82;
        private const byte currentFrame = 0x01;
        private const byte numberOfFrames = 0x01;
        private const int bitCounter = 8;
        public enum MessageType
        {
            Normal_1L = 0,
            Normal_2L = 1,
            Startup = 3,
        }

        public LedPanel(string ipAddress, int connectionPort = 2101)
        {
            this.ipAddress = ipAddress;
            this.connectionPort = connectionPort;
            this.socket  = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void DisplayMessage(string line1, string line2, MessageType messageType)
        {
            var message = new List<byte>();

            switch (messageType)
            {
                case MessageType.Normal_1L:
                    {
                        message.AddRange(new byte[] { command, mediumFontSize, command, alignCenter });
                        break;
                    }
                case MessageType.Normal_2L:
                    {
                        message.AddRange(new byte[] { command, defaultFontSize, command, firstLine });
                        break;
                    }
                case MessageType.Startup:
                    {
                        message.AddRange(new byte[] { command, bigFontSize, command, speedSetter, slow });
                        break;
                    }
                default:
                    break;
            }

            message.AddRange(ASCIIEncoding.ASCII.GetBytes(line1.ToUpper()));
            message.Add(espace);

            if (!string.IsNullOrWhiteSpace(line2) && messageType == MessageType.Normal_2L)
            {
                message.AddRange(new byte[] { command, speedSetter, slow, command, scrollDown, espace});
                message.AddRange(ASCIIEncoding.ASCII.GetBytes(line2.ToUpper()));
                message.AddRange(new byte[] { espace });
            }

            SendToPanel(configureMessage(message).ToArray());
        }

        public void DisplayASingleBigMessage(string line1)
        {
            var message = new List<byte>();

            message.AddRange(new byte[] { command, bigFontSize, command, speedSetter, slow});
            message.AddRange(ASCIIEncoding.ASCII.GetBytes(line1.ToUpper()));
            message.AddRange(new byte[] { espace, espace });

            SendToPanel(configureMessage(message).ToArray());
        }

        private ushort calculeteCheckSum(byte[] frame)
        {
            // Buffer for CheckSum
            ushort checkSum = 0xFFFF;
            // Formula of polynom
            const ushort Polynom = 0x1021;
            // get the frame's size
            ushort frameLength = (ushort)frame.Length;
            // check all bytes
            for (int i = 0; i < frameLength; i++)
            {
                var flag = 0;
                var Char_to_CRC = (ushort)(frame[i] * 256);
                checkSum = (ushort)(checkSum ^ Char_to_CRC);
                // check all bits of the current byte
                for (int bitCounter = 8; bitCounter != 0; bitCounter--)
                {
                    // if the 15 bit of current byte = 1
                    if (checkSum > 32767)
                    {
                        // flag to operate with polynom
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                    checkSum = (ushort)(checkSum * 2);

                    if (flag != 0)
                    {
                        checkSum = (ushort)(checkSum ^ Polynom);
                    }
                }
            }
            return checkSum;
        }

        private List<byte> configureMessage (List<byte> message)
        {
            var messageSize = message.ToArray().Length;
            var frame = new List<byte>();
            frame.AddRange(new byte[] { initiateFrame, initiateMessage, computerCode,
                                        computerGroup, computerId, panelCode, panelGroup,
                                        panelId, quickMessage, currentFrame, numberOfFrames });

            frame.Add((byte)(messageSize >> bitCounter));
            frame.Add((byte)messageSize);

            frame.AddRange(message.ToArray());
            frame.Add(endFrame);

            var checkSum = calculeteCheckSum(frame.ToArray());
            frame.Add((byte)(checkSum >> bitCounter));
            frame.Add((byte)checkSum);

            return frame;
        }

        private void SendToPanel(byte[] frame)
        {
            try
            {
                socket.Connect(ipAddress, connectionPort);
                socket.Send(frame);
                socket.Close();
            }
            catch
            {
                Console.WriteLine("Check your connection!");
            }
            finally
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }
    }
}
