using System;
using System.Configuration;

namespace RestaurantApp.Voting.DesktopApp.Config
{
    /// <summary>
    /// Representa a configuração da aplicação
    /// </summary>
    public class ConfigurationHandler
    {
        public ConfigurationHandler()
        {
            Refresh();
        }

        public readonly TimeSpan DefaultLaunchTime = TimeSpan.FromHours(12);

        public TimeSpan LaunchTime { get; private set; }

        public void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
            var value = ConfigurationManager.AppSettings["LaunchTime"];
            TimeSpan launchTime;

            LaunchTime = !TimeSpan.TryParse(value, out launchTime) || launchTime.Days >= 1
                ? DefaultLaunchTime
                : launchTime;
        }
    }
}
