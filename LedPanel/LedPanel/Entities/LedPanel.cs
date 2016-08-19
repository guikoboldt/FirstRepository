using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LedPanel.Entities
{
    class LedPanel
    {
        private string ipAddress { get; set; }
        private const int connectionPort = 2034;

        public LedPanel(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public void DisplayMessage(string line1)
        {
            DisplayMessage(line1, "");
        }

        public void DisplayMessage (string line1, string line2)
        {

            IList<byte> message = new List<byte>();
            (message as List<byte>).AddRange(new byte[] { 0xAA, 0x01 });
            (message as List<byte>).AddRange(ASCIIEncoding.ASCII.GetBytes(line1));
            (message as List<byte>).AddRange(new byte[] { 0xAA, 0x02 });
            (message as List<byte>).AddRange(ASCIIEncoding.ASCII.GetBytes(line2));

            var messageSize = (ushort)message.ToArray().Length;

            IList<byte> frame = new List<byte>();

            (frame as List<byte>).AddRange(new byte[] { 0x01, 0x02, 0x50, 0x01, 0x01, 0xAA, 0x01, 0x01, 0x82, 0x01, 0x01 }); //start frame

            frame.Add((byte)(messageSize >> 8));
            frame.Add((byte)messageSize);

            (frame as List<byte>).AddRange(message); //add message to packet
            frame.Add(0x03); //frame's end
            var checkSum = calculeteCheckSum(message.ToArray());

            frame.Add((byte)(checkSum >> 8));
            frame.Add((byte)checkSum);

            SendToPanel(frame.ToArray());
        }

        private ushort calculeteCheckSum(byte[] frame)
        {
            // Buffer para valor CRC16 calculado
            ushort CRC16 = 0xFFFF;
            // Polinômio para cálculo do valor CRC16
            ushort Polynom = 0x1021;
            // pega o tamanho do frame
            var frameLength = (ushort)frame.Length;
            // Enquanto não processados todos os caracteres previstos
            for (int i = 0; i < frameLength; i++)
            {
                var polinFlag = 0;
                // Prepara caracter do Buffer para cálculo
                var Char_to_CRC = (ushort)(frame[i] * 256);
                // CRC16 = (Valor atual CRC16 EXOR'ed com caracterdo Buffer
                CRC16 = (ushort)(CRC16 ^ Char_to_CRC);

                // Enquanto não processados todos os bits do caracter atual
               for (int j = 8; j >= 0; j--)
                { 
                    // Se bit 15 do CRC atual = 1
                    if (CRC16 > 32767)
                    {
                        // Sinaliza necessária operação com o POLINÔMIO PADRÃO
                        polinFlag = 1;
                    }
                    // Shift Left valor CRC16 atual
                    CRC16 = (ushort)(CRC16 * 2);
                    // Se bit 15 do CRC atual = 1
                    if (polinFlag != 0)
                    {
                        // ExOR valor CRC16 atual e polinônio padrão
                        CRC16 = (ushort)(CRC16 ^ Polynom);
                    }
                }
            }
            // retorna o resultado do calculo em dois bytes
            return CRC16;
        }

        private void SendToPanel (byte[] frame)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipAddress, connectionPort);
            socket.Send(frame);
            socket.Close();
        }

    }
}
