using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethodsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = Enumerable.Range(1, 100).Select(n => n * 2).Circular().Take(1000).ToArray();

            var list = new List<int>();
            //list.AddRange(from item in new[] { 1, 2, 3, 4, 5, 6 } select item);
            (from item in new[] { 1, 2, 3, 4, 5, 6 } select item).AddTo(list); //more easy to understand

            new[] { 1, 2, 3, 4 }.AddTo(list); //there is no necessity of create a variable of Enumerable type

            list.Publish();
            list.GetElementByIndex(2);
            new List<string> { "a", "b" }.Publish();

            var semana = Dia.Sunday;
            Console.WriteLine("\n {0}", semana);


            Console.ReadLine();
        }
    }

    static public class EnumerableExtensions //create a manipulation class
    {
        static public IEnumerable<T> Circular<T>(this IEnumerable<T> items)
        {
            while (true)
            {
                foreach (var item in items)
                {
                    yield return item; //yield get elements on demand, that let memory free, but also keep the collection in a open mode (database connection, files open)
                }
            }
        }

        static public void AddTo<T>(this IEnumerable<T> items, IList<T> collection)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        static public void Publish<T> (this IList<T> collection)
        {
            foreach (var item in collection)
            {
                Console.Write(" " + item);
            }
        }

        static public T GetElementByIndex<T> (this IList<T> collection, int index)
        {
            if (index > 0 && index < collection.Count)
            {
                return collection.ElementAt(index);
            }
            else if (index < 0)
                index += 1;
            else
                index -= 1;

            return GetElementByIndex(collection, index);
        }
    }
    public enum Dia
    {
        Sunday,
        Monday,
        Friday
    }

}
