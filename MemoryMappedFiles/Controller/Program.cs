using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace Controller
{
    static public class Program
    {
        static public void Main()
        {
            using var mmf = MemoryMappedFile.CreateNew("Client1File.txt", 10000);
            using var mmf2 = MemoryMappedFile.CreateNew("Client2File.txt", 10000);

            var process = new Process();
            process.StartInfo.FileName = @"client1\Client1.exe";
            process.StartInfo.Arguments = "-InMemory";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();

            var process2 = new Process();
            process2.StartInfo.FileName = @"client2\Client2.exe";
            process2.StartInfo.Arguments = "-InMemory";
            process2.StartInfo.UseShellExecute = true;
            process2.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process2.Start();

            Console.ReadLine();
        }
    }
}
