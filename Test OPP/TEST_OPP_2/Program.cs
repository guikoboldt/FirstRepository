using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var link in links)
            {
                Links.Add(link);
            }
        }

        virtual public ICollection<ILink<T>> Links { get; protected set; }

        async virtual public Task WriteRoutesBetweenAsync(T from, T to, Action<IEnumerable<ILink<T>>> add)
        {
            ILink<T>[] rangedLinks = null;
            int count = 0;
            foreach (var link in Links)
            {
                if (from.Equals(link.Source) || (to.ToString().CompareTo(link.Target.ToString())) <= 0)
                {
                    // rangedLinks[count] = link;
                    //   count++;
                }
            }
            //add(rangedLinks);
        }
    }
}
