using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace virtualPanel
{
    public partial class App : Application
    {
        protected async override void OnStartup(StartupEventArgs e)
        {
            ViewModel.MainWindowViewModel vm = new ViewModel.MainWindowViewModel();
            Socket.Socket server = new Socket.Socket();

            //vm._displayMessage = "teste";
            MainWindow = new MainWindow();
            MainWindow.DataContext = vm;
            MainWindow.Show();
            while(true)
                vm.DisplayMessage = await server.GetMessageFromServer();


            //vm.DisplayMessage = await server.GetMessageFromServer();
        }
    }
}
