using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace FileManagerBDD.Extensions
{
    static public class TableExtensions
    {
        static public StringCollection ToStringCollection(this Table table)
        {
            var values = new StringCollection();
            foreach (var row in table.Rows)
            {
                values.Add(row[0]);
            }
            return values;
        }
    }
}
