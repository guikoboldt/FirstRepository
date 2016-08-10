using TechTalk.SpecFlow;

using System;
using System.IO;
namespace RestaurantApp.Voting.IntegrationTests.Hooks
{
    /// <summary>
    /// Representa a class que define os "hooks" necessário para tirar relatórios;
    /// For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
    /// </summary>
    [Binding]
    public class StartUpHooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
        }

        private static string GetAbsolutePath(string path = "")
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
    }
}
