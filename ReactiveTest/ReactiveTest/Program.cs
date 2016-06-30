using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReactiveTest
{
    class Program
    {
        static void Main(string[] args)
        {

            IList<int> temp = new List<int>();
            //var startHour = DateTime.Now;
            //var timer = new Timer(10000);
            //timer.Start();
            var time = DateTime.Now.TimeOfDay.Hours;
            var timenew = time + 1;
            var time2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timenew , 00, 00).TimeOfDay;
            var scheduledTime = DateTime.Now.AddTicks((time2 - DateTime.Now.TimeOfDay).Ticks).TimeOfDay;


            var rand = new Random();

            Observable
                .Interval(TimeSpan.FromSeconds(1)) //throws an event per second
                .Synchronize() //synchronize it
                .Select(_ => rand.Next()) //select the values - performance counter values
                .Buffer(TimeSpan.FromSeconds(10)) //create a buffer of these values by 10 seconds
                .Subscribe(data => //subscribe this buffer after 10s
                {
                    Console.WriteLine();

                    foreach (var item in data)
                    {
                        Console.Write($"{item},");
                    }
                }); //create an event per minute
            
            //Observable
            //    .Interval(TimeSpan.FromMinutes(1))
            //    .Select(_ => rand.Next()); //create an event per minute

            ////var valuesObservable = temp.ToObservable().Do Synchronize(); //criate an Iobservable List from temp list and synchronize it

            //eventPerMinute.Subscribe(time =>
            //{
                
            //});


            //var eventPersecond = Observable.Interval(TimeSpan.FromSeconds(10));

            //eventPersecond.Subscribe(time =>
            //   {
            //       temp.Add(count++);
            //   });

            //eventStream = (from e in eventStream
            //               let time = DateTime.Now
            //               where time.ToLongTimeString().Equals(startHour.AddMinutes(1).ToLongTimeString())
            //               select e);
            //var observableList = temp.ToObservable();
            //eventStream.Subscribe(time =>
            //                   {
            //                        Console.WriteLine("Start Hour: {0}, Counter: {1} ", startHour.ToLongTimeString(), count);
            //                        startHour = DateTime.Now; 
            //                   },
            //                    error => Console.WriteLine("Error: {0} ", error.Message),
            //                    () => Console.WriteLine("onComplete"));

            Console.ReadKey();
        }
    }
}
