using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dm = new Entities.FileManager(@"C:\Download", @"C:\Download_copy");
            //var dm_final = new Entities.FileManager(@"C:\Download_copy", @"C:\Download_copy2");

            Console.Read();
        }
    }
}
