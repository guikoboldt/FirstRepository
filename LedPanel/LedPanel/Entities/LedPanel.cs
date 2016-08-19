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
        private int connectionPort { get; set; } = 2101; //default panel's connecttion port 2101;

        private const byte espace = 0x20;
        private const byte command = 0xAA;
        private const byte defaultFontSize = 0x11;
        private const byte bigFontSize = 0x16;
        private const byte firstLine = 0x01;
        private const byte secondLine = 0x02;
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

        public LedPanel(string ipAddress)
        {
            this.ipAddress = ipAddress;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(ipAddress, connectionPort);
        }

        public IDisposable DisplayMessage(string line1, string line2 = "")
        {
            var message = new List<byte>();

            message.AddRange(new byte[] { command, defaultFontSize, command, firstLine });
            message.AddRange(ASCIIEncoding.ASCII.GetBytes(line1));

            if (!string.IsNullOrWhiteSpace(line2))
            {
                message.AddRange(new byte[] { command, secondLine });
                message.AddRange(ASCIIEncoding.ASCII.GetBytes(line2));
            }

            var messageSize = (ushort)message.ToArray().Length;

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

            return SendToPanel(frame.ToArray());
        }
        private ushort calculeteCheckSum(byte[] frame)
        {
            // Buffer para valor CRC16 calculado
            ushort checkSum = 0xFFFF;
            // Polinômio para cálculo do valor CRC16
            const ushort Polynom = 0x1021;
            // pega o tamanho do frame
            ushort frameLength = (ushort)frame.Length;
            // Enquanto não processados todos os caracteres previstos
            for (int i = 0; i < frameLength; i++)
            {
                var flag = 0;
                // Prepara caracter do Buffer para cálculo
                var Char_to_CRC = (ushort)(frame[i] * 256);
                // CRC16 = (Valor atual CRC16 EXOR'ed com caracterdo Buffer
                checkSum = (ushort)(checkSum ^ Char_to_CRC);
                // Enquanto não processados todos os bits do caracter atual
                for (int bitCounter = 8; bitCounter != 0; bitCounter--)
                {
                    // Se bit 15 do CRC atual = 1
                    if (checkSum > 32767)
                    {
                        // Sinaliza necessária operação com o POLINÔMIO PADRÃO
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                    // Shift Left valor CRC16 atual
                    checkSum = (ushort)(checkSum * 2);
                    // Se bit 15 do CRC atual = 1
                    if (flag != 0)
                    {
                        // ExOR valor CRC16 atual e polinônio padrão
                        checkSum = (ushort)(checkSum ^ Polynom);
                    }
                }
            }
            return checkSum;
        }

        private IDisposable SendToPanel(byte[] frame)
        {
            try
            {
                socket.Send(frame);
            }
            catch
            { }
            return socket;
        }

        public void Dispose()
        {
            ((IDisposable)socket)?.Dispose();
        }

        public static void SendMessage(string ipAddress, string line1, string line2 = "")
        {
            using (var panel = new Entities.LedPanel(ipAddress))
                panel.DisplayMessage(line1, line2);
        }
    }
}
