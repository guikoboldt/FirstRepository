using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client2
{
    static public class Program
    {
        static public void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.FirstOrDefault().Equals("-InMemory"))
                {
                    Console.WriteLine("Starting InMemory client 2");

                    var fileToNotifyAlives = MemoryMapped.InMemoryMappedFile.CreateFromExistent("Client2File.txt");
                    var fileToReceive = MemoryMapped.InMemoryMappedFile.CreateFromExistent("Client1File.txt");

                    WatchdogMemoryMapped.WatchdogProcess.InitializeInMemoryWatchdogService(watchdogTime: 1000,
                                                                                           fileToNotifyAlives: fileToNotifyAlives,
                                                                                           fileNameToReceive: fileToReceive,
                                                                                           onDeadMessageReceived: () => Task.FromResult(true));
                }
            }
            else
            {
                Console.WriteLine("Starting client 2");

                var fileToNotifyAlives = "Client2File.txt";
                WatchdogMemoryMapped.WatchdogProcess.InitializeWatchdogService(watchdogTime: 1000,
                                                                               fileToNotifyAlives: fileToNotifyAlives,
                                                                               filePathToReceiveAlives: @"C:\GSC\FirstRepository\MemoryMappedFiles\Client1\bin\Debug\netcoreapp3.1\files\Client1File.txt",
                                                                               fileNameToReceive: "Client1File.txt",
                                                                               onDeadMessageReceived: () => Task.FromResult(true));
            }
            Console.ReadLine();
        }
    }
}
