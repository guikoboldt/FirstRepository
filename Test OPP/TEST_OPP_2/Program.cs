using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Console.WriteLine();
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
        async virtual public Task WriteRoutesBetweenAsync(T from, T to, Action<IEnumerable<ILink<T>>> add, IEnumerable<ILink<T>> links, IEnumerable<ILink<T>> checkedlinks)
        {
            var validLinks = (from link in links
                              where link.Source.Equals(@from)
                              select link).ToArray();

            var otherLinks = (from link in links
                              where !link.Source.Equals(@from)
                              select link).ToArray();

            foreach (var item in validLinks)
            {
                checkedlinks = checkedlinks.Concat(new ILink<T>[] { item }).ToArray();

                if (!item.Target.Equals(to))
                {
                    await WriteRoutesBetweenAsync(item.Target, to, add, otherLinks, checkedlinks);
                }
                else
                {
                    add(checkedlinks);
                }

                if (checkedlinks.Last().Equals(item))
                {
                    checkedlinks = checkedlinks.TakeWhile(o => !o.Equals(checkedlinks.Last())).ToArray();
                }
                else if (!checkedlinks.Last().Target.Equals(item.Source))
                {
                    checkedlinks = new ILink<T>[] { };
                }
            }
        }

        async virtual public Task WriteRoutesBetweenAsync(T from, T to, Action<IEnumerable<ILink<T>>> add)
        {
            await WriteRoutesBetweenAsync(from, to, add, Links.ToArray(), new ILink<T>[] { });
        }


    }
}
