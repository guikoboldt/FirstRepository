using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Test");
            //int a = 1;
            //Console.WriteLine(a);
            //Console.ReadLine();

            //List<string> test = new List<string>(from a in "The quick brown fox jumps over the lazy dog".Split() orderby a.Length select a);
            //from variable in table orderby select
            System.IO.StreamReader file = new System.IO.StreamReader("teste.txt"); //read the file and put it into file variable
            int countLine = 0;
            string line;
            string[] statements = new string[1];

            while ((line = file.ReadLine()) != null)
            { 
                countLine++;
            }
            file.Close();

            
            statements = new string[countLine];

            file = new System.IO.StreamReader("teste.txt");
            countLine = 0;

            while ((line = file.ReadLine()) != null)
            {
                statements[countLine] = line;
                countLine++;
            }

            //string b = "Hey you!";
            List<string> test = new List<string>(from a in statements where statements[0] == a select a); //from variable location where select variable
            foreach (var text in test)
            {
                Console.WriteLine(text);
            }
            
            Console.ReadLine();

        }
    }
}
