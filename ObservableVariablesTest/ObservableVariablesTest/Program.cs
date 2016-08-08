using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Data.SqlClient;

namespace ObservableVariablesTest
{
    public class Program
    {
        static event EventHandler click;
        static string connectionStrings = System.Configuration.ConfigurationManager.ConnectionStrings["TEST_OPPEntities"].ConnectionString;

        static void Main(string[] args)
        {
            var i = 0;
            Observable
                .FromEventPattern(
                    _ => click += _,
                    _ => click -= _
                    )
                .Buffer(TimeSpan.FromMilliseconds(200),5000)
                .Subscribe(data =>
                {
                    SqlCommand command = new SqlCommand();
                    Console.WriteLine(data.Count);
                    i++;
                    foreach (var item in data)
                    {
                        //using (var db = new ObservableVariablesTest.Database.TEST_OPPEntities())
                        //{
                        //    db.VariablesValuesSet.Add(new Database.VariablesValuesSet
                        //    {
                        //        idData = i,
                        //        ValueSender = "Sender " + i,
                        //        ValueArgs = "Args " + i,
                        //    });
                        //    db.SaveChanges();
                        //}
                        
                        command.CommandText += string.Format("insert into VariablesValuesSet (idData, ValueSender, ValueArgs) values (\'{0}\', \'Sender{0}\', \'Args{0}\');", i);
                    }

                    using (var connection = new SqlConnection(connectionStrings))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                });

            var tasks = (from n in Enumerable.Range(0, 10)
             select Task.Run(() =>
             {
                 while (true)
                 {
                     Thread.Sleep(1);
                     click(null, null);
                 }
             })).ToArray();

            Task.WaitAll(tasks);

        }
    }
}
