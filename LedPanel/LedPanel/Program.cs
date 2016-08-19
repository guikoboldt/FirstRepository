using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LedPanel
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var timer = new Timer(5000);
            using (var panel = new Entities.LedPanel("localhost"))
            {
                timer.Elapsed += (e, s) => { panel.DisplayMessage(line1: "CAMINHAO: " + random.Next(1, 20), line2: "LINHA: " + random.Next(1, 20)); };

                timer.Start();

                timer.Enabled = true;

                Console.Read();
            }
            //using (var panel = new Entities.LedPanel("192.168.1.2"))
            //{
            //    //new Program();
            //    while (true)
            //    {
            //        Console.WriteLine("Digite a frase 1:");
            //        var message1 = Console.ReadLine();
            //        Console.WriteLine("Digite a frase 2:");
            //        var message2 = Console.ReadLine();
            //        panel.DisplayMessage(message1, message2);
            //    }
            //}
        }

        //public Program()
        //{
        //    //classe de origem, neste caso o computador
        //    const byte COMPUTADOR = 0x50;
        //    //classe de destino, neste caso o painel de mensagens
        //    const byte MSG = 0xAA;

        //    //converte a mensagens em um array de bytes
        //    byte[] message = ASCIIEncoding.ASCII.GetBytes("bla");

        //    //monta o frame para atualizar a mensagem do painel
        //    byte[] frame = createFrameCrc(COMPUTADOR, //computador como classe de origem
        //                                           0x01, //ID do grupo de origem
        //                                           0x01, //ID do dispositivo de origem
        //                                           MSG,  //painel de mensagens como classe de destino
        //                                           0x01, //ID do grupo de destino
        //                                           0x01, //ID do dispositivo de destino
        //                                           0x82, //comando de atualizar o texto apresentado
        //                                           0x01, //frame atual
        //                                           0x01, //total de frames
        //                                           message); //mensagem a ser apresentada no painel

        //    //objeto para conexão com o painel
        //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    //abre a conexão passando o ip e a porta de rede do painel
        //    socket.Connect("192.168.1.2", 2101);
        //    //envia o frame
        //    socket.Send(frame);
        //    //fecha a conexão
        //    socket.Close();


        //    Console.Write("Pressione qualquer tecla para sair...");
        //    Console.ReadLine();
        //}

        //public byte[] createFrameCrc(byte sourceClass,
        //                                byte sourceGroup,
        //                                byte sourceId,
        //                                byte destClass,
        //                                byte destGroup,
        //                                byte destID,
        //                                byte command,
        //                                byte currFrame,
        //                                byte totFrames,
        //                                byte[] data)
        //{
        //    //caracteres de controle (tabela ascii)
        //    const byte SOH = 0x01; //inicio de frame
        //    const byte STX = 0x02; //inicio de texto
        //    const byte ETX = 0x03; //fim de texto

        //    //lista para criar o array de bytes/frame
        //    List<byte> BufferFrame = new List<byte>();

        //    // pega o quantidade de bytes de dados que serão enviados
        //    ushort lenData = (ushort)data.Length;
        //    // inicializa a variavel onde será calculado o crc
        //    ushort crc = 0;

        //    // header do protocolo
        //    BufferFrame.Add(SOH);
        //    BufferFrame.Add(STX);

        //    // informações da origem do frame
        //    BufferFrame.Add(sourceClass);
        //    BufferFrame.Add(sourceGroup);
        //    BufferFrame.Add(sourceId);

        //    // informações do destino do frame
        //    BufferFrame.Add(destClass);
        //    BufferFrame.Add(destGroup);
        //    BufferFrame.Add(destID);

        //    // comando do frame
        //    BufferFrame.Add(command);
        //    // controle de sequencia e quantidade de pacotes
        //    BufferFrame.Add(currFrame);
        //    BufferFrame.Add(totFrames);

        //    // quantidade de dados do frame
        //    BufferFrame.Add((byte)(lenData >> 8));
        //    BufferFrame.Add((byte)lenData);

        //    // dados do frame
        //    BufferFrame.AddRange(data);

        //    // footer do frame
        //    BufferFrame.Add(ETX);

        //    // calculo do crc
        //    crc = calcCRC(BufferFrame.ToArray());

        //    // inclusão do calculo 
        //    BufferFrame.Add((byte)(crc >> 8));
        //    BufferFrame.Add((byte)crc);

        //    // retorna o frame montado
        //    return BufferFrame.ToArray();
        //}

        //public ushort calcCRC(byte[] frame)
        //{
        //    // Buffer para valor CRC16 calculado
        //    ushort CRC16 = 0xFFFF;
        //    // Buffer para operar caracteres durante cálculo CRC16
        //    ushort Char_to_CRC;
        //    // Contador de bits do caracter a adicionar ao cálculo CRC16
        //    ushort Bit_Counter;
        //    // Flag para sinalizar se necessário cálcular com Polinômio
        //    ushort Polin_Flag;
        //    // Polinômio para cálculo do valor CRC16
        //    const ushort Polynom = 0x1021;
        //    // inicializa o contador
        //    ushort i = 0;
        //    // pega o tamanho do frame
        //    ushort LenFrame = (ushort)frame.Length;

        //    // Enquanto não processados todos os caracteres previstos
        //    while (LenFrame != 0)
        //    {
        //        // Contador de bits de cada caracter a ser processado
        //        Bit_Counter = 8;
        //        // Prepara caracter do Buffer para cálculo
        //        Char_to_CRC = (ushort)(frame[i] * 256);
        //        // CRC16 = (Valor atual CRC16 EXOR'ed com caracterdo Buffer
        //        CRC16 = (ushort)(CRC16 ^ Char_to_CRC);

        //        // Enquanto não processados todos os bits do caracter atual
        //        while (Bit_Counter != 0)
        //        {
        //            // Se bit 15 do CRC atual = 1
        //            if (CRC16 > 32767)
        //            {
        //                // Sinaliza necessária operação com o POLINÔMIO PADRÃO
        //                Polin_Flag = 1;
        //            }

        //            else
        //            {
        //                // Sinaliza desnecessário operação com o POLINÔMIO PADRÃO
        //                Polin_Flag = 0;
        //            }

        //            // Shift Left valor CRC16 atual
        //            CRC16 = (ushort)(CRC16 * 2);

        //            // Se bit 15 do CRC atual = 1
        //            if (Polin_Flag != 0)
        //            {
        //                // ExOR valor CRC16 atual e polinônio padrão
        //                CRC16 = (ushort)(CRC16 ^ Polynom);
        //            }

        //            // Decrementa contador de bits do caracter atual
        //            Bit_Counter--;
        //        }

        //        // Prepara ponteiro para o próximo caracter a processar
        //        i++;
        //        // Decrementa contador de caractertes a processar
        //        LenFrame--;

        //    }

        //    // retorna o resultado do calculo em dois bytes
        //    return CRC16;
        //}
    }
}
