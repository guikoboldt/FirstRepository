﻿using ADayOfBets.Resources;
using ADayOfBets.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace teste
{
    class Program
    {
        //static public IEnumerable<string> getLines(string path)
        //{
        //    using (var file = new System.IO.StreamReader(path)) //read the file and put it into file variable
        //    {
        //        var line = default(string); //create a general variable, which run as string bt default

        //        while ((line = file.ReadLine()) != null)
        //        {
        //            yield return line; //keep the method running and return item by item, in this case line by line
        //        } //this structure just works with IEnumerable, because of the iterator
        //    }
        //}

        static void Main(string[] args) {




            MainWindow test = new MainWindow();
            test.Show();
            //Strings.Culture = new System.Globalization.CultureInfo("pt-BR"); //define culture language
            //Console.WriteLine(Strings.UserName); //calls the UserName key on Strings file on Resources Folder
            //Console.ReadLine();
        //    var path = teste.Resources.Strings.TestFileName;
        //    Strings.Culture = new System.Globalization.CultureInfo("pt-BR");
        //    var foto = Strings.MyPhoto;
        //    foreach (var line in getLines(path))
        //    {
        //        Console.WriteLine(line);
        //    }

        //    var statements = getLines(path).ToArray();
        //    ////string b = "Hey you!";
        //    //List<string> test = new List<string>(from a in statements  select a); //from - variable - location - where - select
        //    //foreach (var text in test)
        //    //{
        //    //    Console.WriteLine(text);
        //    //}

        //    Console.WriteLine("Update from home");

        //    Console.ReadLine();

        }
    }
}
