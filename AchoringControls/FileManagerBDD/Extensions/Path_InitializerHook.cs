using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace FileManagerBDD.Extensions
{
    [Binding]
    public class Path_InitializerHook
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario("Path_initializer")]
        public void PathInitializer()
        {
            if (!Directory.Exists(@"C:\Download"))
                Directory.CreateDirectory(@"C:\Download");

            var files = new DirectoryInfo(@"C:\Download").GetFiles();
            if (files.Length == 0)
            {
                File.Create(@"C:\Download\text 0.txt");
                File.Create(@"C:\Download\text 1.txt");
                File.Create(@"C:\Download\text 2.txt");
            }
        }

        [BeforeScenario("Path_IsEmpty")]
        public void PathIsEmpty()
        {
            var files = new DirectoryInfo(@"C:\Download").GetFiles();
            foreach (var file in files)
            {
                System.GC.Collect(); //force a garbage collection
                System.GC.WaitForPendingFinalizers(); //suspend this thread until complete the pending another thread
                file.Delete();
            }
        }
    }
}
