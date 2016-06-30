using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_TESTE.Utilities
{
    class Characteres
    {
        private string name { set; get; }
        private string race { set; get; }
        private int level { set; get; }

        public Characteres (string name, string race, int level)
        {
            this.name = name;
            this.race = race;
            this.level = level;
        }

    }
}
