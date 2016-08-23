using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LedPanel.Entities
{
    public class LedPanel : IDisposable
    {
        public Socket socket { get; set; }
        private string ipAddress { get; set; }
        private int connectionPort { get; set; } //default panel's connection port: 2101;

        public enum FrameTokens
        {
            Space = 0x20,
            Command = 0xAA,
            DefaultFontSize = 0x11,
            BigFontSize = 0x16,
            MediumFontSize = 0x13,
            FirstLine = 0x01,
            SecondLine = 0x02,
            AlignCenter = 0x04,
            SpeedSetter = 0x40,
            Slow = 0x0A,
            ScrollDown = 0x24,
            ScrollUp = 0x23,
            InitiateFrame = 0x01,
            InitiateMessage = 0x02,
            EndFrame = 0x03,
            ComputerCode = 0x50,
            ComputerGroup = 0x01,
            ComputerId = 0x01,
            PanelCode = 0xAA,
            PanelGroup = 0x01,
            PanelId = 0x01,
            QuickMessage = 0x82,
            CurrentFrame = 0x01,
            NumberOfFrames = 0x01,
            BitCounter = 8,
        } //tokens to configure message (fontSize, scroll speed, ...)
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
        }

        async private Task<Socket> GetSocket()
        {
            if (this.socket != null)
                return this.socket;

            return await Task.Run(() =>
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(ipAddress, connectionPort);
                return this.socket;
            });
        }

        async public Task DisplayMessage(string line1, string line2, MessageType messageType)
        {
            var message = new List<byte>();

            switch (messageType)
            {
                case MessageType.Normal_1L:
                    {
                        message.AddRange(new byte[] { (byte)FrameTokens.Command, (byte)FrameTokens.MediumFontSize, (byte)FrameTokens.Command, (byte)FrameTokens.AlignCenter });
                        break;
                    }
                case MessageType.Normal_2L:
                    {
                        message.AddRange(new byte[] { (byte)FrameTokens.Command, (byte)FrameTokens.DefaultFontSize, (byte)FrameTokens.Command, (byte)FrameTokens.FirstLine });
                        break;
                    }
                case MessageType.Startup:
                    {
                        message.AddRange(new byte[] { (byte)FrameTokens.Command, (byte)FrameTokens.BigFontSize, (byte)FrameTokens.Command, (byte)FrameTokens.SpeedSetter, (byte)FrameTokens.Slow });
                        break;
                    }
                default:
                    break;
            }

            message.AddRange(ASCIIEncoding.ASCII.GetBytes(line1.ToUpper()));
            message.Add((byte)FrameTokens.Space);

            if (!string.IsNullOrWhiteSpace(line2) && messageType == MessageType.Normal_2L)
            {
                message.AddRange(new byte[] { (byte)FrameTokens.Command, (byte)FrameTokens.SpeedSetter, (byte)FrameTokens.Slow, (byte)FrameTokens.Command, (byte)FrameTokens.ScrollDown, (byte)FrameTokens.Space });
                message.AddRange(ASCIIEncoding.ASCII.GetBytes(line2.ToUpper()));
                message.AddRange(new byte[] { (byte)FrameTokens.Space });
            }

            await SendToPanel(configureMessage(message).ToArray());
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
                for (int bitCounter = (byte)FrameTokens.BitCounter; bitCounter != 0; bitCounter--)
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

        private List<byte> configureMessage(List<byte> message)
        {
            var messageSize = message.ToArray().Length;
            var frame = new List<byte>();
            frame.AddRange(new byte[] { (byte)FrameTokens.InitiateFrame, (byte)FrameTokens.InitiateMessage, (byte)FrameTokens.ComputerCode,
                                        (byte)FrameTokens.ComputerGroup, (byte)FrameTokens.ComputerId, (byte)FrameTokens.PanelCode, (byte)FrameTokens.PanelGroup,
                                        (byte)FrameTokens.PanelId, (byte)FrameTokens.QuickMessage, (byte)FrameTokens.CurrentFrame, (byte)FrameTokens.NumberOfFrames });

            frame.Add((byte)(messageSize >> (byte)FrameTokens.BitCounter));
            frame.Add((byte)messageSize);

            frame.AddRange(message.ToArray());
            frame.Add((byte)FrameTokens.EndFrame);

            var checkSum = calculeteCheckSum(frame.ToArray());
            frame.Add((byte)(checkSum >> (byte)FrameTokens.BitCounter));
            frame.Add((byte)checkSum);

            return frame;
        }

        async private Task SendToPanel(byte[] frame)
        {
            await Task.Run(async () =>
           {
               var socket = await this.GetSocket();
               socket.Send(frame);
           });
        }
        public void Dispose()
        {
            ((IDisposable)this.socket)?.Dispose();
        }

        async public static Task SendMessage(string ipAdderss, int connectionPort, string line1, string line2, MessageType messageType)
        {
            using (var panel = new LedPanel(ipAdderss, connectionPort))
                await panel.DisplayMessage(line1, line2, messageType);
        }

        async public static Task SendMessage(string ipAdderss, string line1, string line2, MessageType messageType)
        {
            using (var panel = new LedPanel(ipAdderss))
                await panel.DisplayMessage(line1, line2, messageType);
        }
    }
}
