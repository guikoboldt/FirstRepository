using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.Devices;
using System.ComponentModel;
using ShowDiagnostics.Views;
using ShowDiagnostics.Utilitys;

namespace ShowDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel = new ViewModel();
        private ComputerCounter computer = new ComputerCounter();
        private int logTime;
        private Timer update = new Timer(10000); //10 s
        int startHour; 

        public MainWindow()
        {
            DataContext = viewModel;
            computer.CheckFileLog(); //check if fileLog already exists
            computer.SetDefaultValues(); //iniciate all variables with default values
            startHour = DateTime.Now.Hour;

            InitializeComponent();

            update.Enabled = true; //enable timer
            update.Elapsed += (s, e) => ShowInformation();

            if (DateTime.Now.Hour.Equals(startHour + 1))
            {
                update.Stop();
                //computer.SaveLog(logTime);
                //Console.WriteLine("Start Hour: {0} , Final Hour: {1} ##", startHour);
                startHour = DateTime.Now.Hour;
            }
            update.Start();
        }

        public void ShowInformation()
        {
            viewModel.Cpu = computer.GetCpuInformation();
            viewModel.Ram = computer.GetRamInformation();
            viewModel.Disk = computer.GetDiskActive();
            var networkSent = computer.GetNetworkInfoSent();
            var networkReceived = computer.GetNetworkInfoReceived();
            viewModel.NetworkSent = networkSent[0];
            viewModel.NetworkReceived = networkReceived[0];
            Console.WriteLine("Start Hour: {0} ##", startHour);

            logTime++;
            if (logTime % 10 == 0)
            {
                computer.SaveLog(logTime);
            }
        }
    }
}
