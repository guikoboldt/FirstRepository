using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticsNew
{
    class Program
    {
        static readonly string[] Categories = new string[]
        {
            //"Memory",
            "Network Interface",
            "PhysicalDisk",
            "Processor",
        };
        static ICollection<PerformanceCounter> counters = new List<PerformanceCounter>();

        static void Main(string[] args)
        {
            var performanceCounterCategories = PerformanceCounterCategory.GetCategories();

            var query = (from category in performanceCounterCategories
                         group category by category.CategoryName into @group
                         where Categories.Contains(@group.Key)
                         orderby @group.Key
                         select new { Key = @group.Key, Values = @group.ToArray() }).ToArray();

            using (var logWriter = new System.IO.StreamWriter("performanceInfo.txt", true))
            {
                foreach (var item in query)
                {
                    Console.WriteLine("{0}: {1} ###########", item.Key, item.Values.Length); //item.key = categoryName

                    //logWriter.WriteLine(item.Key);
                    Console.WriteLine("");
                    Console.WriteLine("");

                    foreach (var value in item.Values)
                    {
                        var category = value.ReadCategory();
                        var interfaces = value.GetInstanceNames();
                        foreach (string counterName in category.Keys)
                        {
                            if (interfaces.Length > 0)
                            {
                                foreach (var instanceName in interfaces)
                                {
                                    Console.WriteLine("{0} : {1} : {2}", item.Key, counterName, instanceName); //item.key = CategoryName, categoryKey = counterName, interfacesName = interfaceName
                                    logWriter.WriteLine(item.Key + ";" + counterName + ";" + instanceName);

                                    counters.Add(new PerformanceCounter(categoryName: item.Key, counterName: counterName, instanceName: instanceName));
                                }
                            }
                            else
                            {
                                Console.WriteLine("{0} : {1}", item.Key, counterName); //item.key = CategoryName, itens = counterName
                                counters.Add(new PerformanceCounter(categoryName: item.Key, counterName: counterName));
                            }
                        }
                        Console.WriteLine("");

                        //var instance = value.GetInstanceNames(); //category = instanceName

                        //for (int i = 0; i < instance.Length; i++)
                        //{
                        //    Console.WriteLine(instance[i]);
                        //    logWriter.WriteLine(instance[i]);
                        //    Console.WriteLine("");

                        //    var counter = value.GetCounters(instance[i]); //get all counter by instanceName

                        //    for (int j = 0; j < counter.Length; j++)
                        //    {
                        //        var counterName = counter[j].CounterName; //get the counterName of all counters by instanceName
                        //        Console.WriteLine(counterName);
                        //        logWriter.WriteLine(counterName);
                        //    }
                        //    Console.WriteLine("");
                        //}
                    }
                }
                while (true)
                {
                    System.Threading.Thread.Sleep(500);
                    foreach (var counter in counters)
                    {
                        logWriter.WriteLine("{0} {1} {2}", counter.CategoryName, counter.CounterName, counter.InstanceName);
                        logWriter.WriteLine("{0:F4}", counter.NextValue());
                        logWriter.WriteLine("");
                    }
                }
            }

            Console.Read();
        }
    }
}
