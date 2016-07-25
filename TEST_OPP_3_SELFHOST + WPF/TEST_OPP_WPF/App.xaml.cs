using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TEST_OPP_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        async void Application_Startup(object sender, StartupEventArgs a)
        {
            Entities.GlobalInformations.StartServerConnection();

            MainWindow mainwindow = new MainWindow(); //create the main window
            Current.MainWindow = mainwindow; //set as the current window

            App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose; //change the shutdown mode of the app

            Window LoginWindow = new Window //create the login dialog
            {
                Title = "Login",
                Height = 80,
                Width = 190,
                Content = new User_Controls.Login(),
                ResizeMode = 0, //no resize
                Background = Brushes.Gray,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowStyle = WindowStyle.None,
            };

            Views.LoginViewModel.OnRequestClose += (s, e) =>
            {
                mainwindow = new MainWindow();
                LoginWindow.Close();
                mainwindow.Show();
            }; //register the onClose event to login dialog by the view

            LoginWindow.ShowDialog(); //show the login window
            await StartHub();
        }

        private async Task StartHub()
        {
           await Entities.GlobalInformations.StartHubConnection();
        }
    }
}
