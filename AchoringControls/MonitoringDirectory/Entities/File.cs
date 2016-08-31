using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class File
    {
        public string name { get; set; }
        public int size { get; set; }
        public DateTime lastChange { get; set; }
        public string location { get; set; }

        public File(string name, int size, string location)
        {
            this.name = name;
            this.size = size;
            this.location = location;
        }
    }
}
