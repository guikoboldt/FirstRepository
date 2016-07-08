using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_OPP_2
{
    class Program
    {
        static public void Main(string[] args)
        {
            Run();
            Console.Read();
        }

        async static private Task Run()
        {
            var links = new ILink<string>[]
            {
                new Link<string>("a","b"),
                new Link<string>("b","c"),
                new Link<string>("c","d"),
                new Link<string>("d","e"),
                new Link<string>("d","a"),
                new Link<string>("a","h"),
                new Link<string>("h","g"),
                new Link<string>("g","f"),
                new Link<string>("f","e"),
                new Link<string>("a", "e"),
                new Link<string>("d", "f"),
                new Link<string>("f", "g"),
                new Link <string>("g", "h"),
            };

            var graph = new Graph<string>(links);

            await graph.WriteRoutesBetweenAsync("a", "e", Print);
        }

        static private void Print(IEnumerable<ILink<string>> links)
        {
            Console.WriteLine(String.Join(", ", links.Select(link => String.Format("({0} -> {1})", link.Source, link.Target))));
        }
    }

    public interface ILink<T>
    {
        T Source { get; }
        T Target { get; }
    }

    public class Link<T> : ILink<T>
    {
        public Link(T source, T target)
        {
            Source = source;
            Target = target;
        }

        public T Source { get; private set; }

        public T Target { get; private set; }
    }

    public interface IGraph<T>
    {
        ICollection<ILink<T>> Links { get; }
        Task WriteRoutesBetweenAsync(T from, T to, Action<IEnumerable<ILink<T>>> add);
    }

    public class Graph<T> : IGraph<T>
    {
        public Graph(IEnumerable<ILink<T>> links)
        {
            IList<ILink<T>> newLinks = new List<ILink<T>>();
            foreach (var link in links)
            {
                try
                {
                    newLinks.Add(link);
                }
                catch
                {
                    Console.WriteLine("Error on add new link");
                }
            }
            Links = newLinks;
        }

        virtual public ICollection<ILink<T>> Links { get; protected set; }

        async virtual public Task WriteRoutesBetweenAsync(T from, T to, Action<IEnumerable<ILink<T>>> add)
        {
            IList<ILink<T>> list = new List<ILink<T>>();
            bool status = false;
            var TargeLinks = from a in Links //get all links where Target attribut is equals the target especified as parameter
                       where a.Target.Equals(@to.ToString())
                       select a;

            foreach (var link in TargeLinks)
            {
                list.Add(link);
                var route = link;
                while (!route.Source.Equals(@from.ToString())) //get all routes available and stop when the source is equals the source especified as parameter
                {
                    route = (from a in Links
                             where route.Source.Equals(a.Target)
                             select a).FirstOrDefault();
                    if (route == null || list.Contains(route))
                        break; //if some is null or the same as the starter link, break the loop
                    else
                        list.Add(route);
                }
                if (list.Last().Source.Equals(@from.ToString())) //if is a valid route
                {
                    var newList = list.Reverse(); //change the order, to print in the correct order
                    add(newList); //calls the add Action
                    status = true;
                }
                list.Clear();
            }
            if (!status) //check if exist some route available
            {
                Console.WriteLine(" No Routes Available between {0} -> {1}", from.ToString(), to.ToString());
            }
        }


    }
}
