using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client1
{
    static public class Program
    {
        static public void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.FirstOrDefault().Equals("-InMemory"))
                {
                    Console.WriteLine("Starting InMemory client 1");

                    var fileToNotifyAlives = MemoryMapped.InMemoryMappedFile.CreateFromExistent("Client1File.txt");
                    var fileToReceive = MemoryMapped.InMemoryMappedFile.CreateFromExistent("Client2File.txt");

                    WatchdogMemoryMapped.WatchdogProcess.InitializeInMemoryWatchdogService(watchdogTime: 1000,
                                                                                           fileToNotifyAlives: fileToNotifyAlives,
                                                                                           fileNameToReceive: fileToReceive,
                                                                                           onDeadMessageReceived: () => { Console.WriteLine("Perdemos o client 2"); return Task.CompletedTask; });
                }
            }
            else
            {
                Console.WriteLine("Starting client 1");

                var fileToNotifyAlives = "Client1File.txt";
                WatchdogMemoryMapped.WatchdogProcess.InitializeWatchdogService(watchdogTime: 1000,
                                                                               fileToNotifyAlives: fileToNotifyAlives,
                                                                               filePathToReceiveAlives: @"C:\GSC\FirstRepository\MemoryMappedFiles\Client2\bin\Debug\netcoreapp3.1\files\Client2File.txt",
                                                                               fileNameToReceive: "Client2File.txt",
                                                                               onDeadMessageReceived: () => Task.FromResult(true));
            }
            Console.ReadLine();
        }
    }
}
