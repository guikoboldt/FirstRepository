using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            for (int i = 0; i < 3; i++)
            {
                File.Create(Path.Combine(@"C:\Download", @"text" + i + ".txt"));
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
