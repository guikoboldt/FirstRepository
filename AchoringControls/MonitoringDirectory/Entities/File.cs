using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class File
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime LastChange { get; set; }
        public string Location { get; set; }

        public File(string name, string location, long size = 0)
        {
            this.Name = name;
            this.Location = location;
            this.Size = size;
        }

        public string FullPath()
        {
            return string.Concat(this.Location, this.Name);
        }
    }
}
