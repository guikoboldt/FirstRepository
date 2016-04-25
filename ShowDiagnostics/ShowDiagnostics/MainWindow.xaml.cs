using System;
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
using System.Threading;

namespace ShowDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            cpuInformation.Text = GetCpuInformation() + " %";
            ramInformation.Text = GetRamInformation() + " GB";
            diskInfo.Text = GetDiskActive() + " %";
            networkInfoSend.Text = GetNetworkInfoSend() + " kb/sec";
            networkInfoReceived.Text = GetNetworkInfoReceived() + " Mb/sec";
        }




        public string GetRamInformation ()
        {
            PerformanceCounter ramInfo = new PerformanceCounter("Memory", "Available MBytes");
            var ramAvailableInfo = Math.Round(( (Double) ramInfo.NextValue() / 1024), 2); //amount memory available in GB

            ComputerInfo cp = new ComputerInfo();
            var ramTotalInfo = Math.Round((((( (Double) cp.TotalPhysicalMemory) / 1024) / 1024) / 1024), 2); //Amount memory installed in GB

            return (ramTotalInfo - ramAvailableInfo).ToString();
        }

        public string GetCpuInformation ()
        {
            PerformanceCounter cpuInfo = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var cpuInformation = cpuInfo.NextValue();
            while (cpuInformation == 0)
            {
                Thread.Sleep(50); //miliseconds
                cpuInformation = cpuInfo.NextValue();
            }
            cpuInformation = (float) Math.Round(cpuInformation, 0);
            return cpuInformation.ToString();
        }

        public string GetDiskActive()
        {
            var diskInfo = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
            var diskInformation = diskInfo.NextValue();
            while (diskInformation == 0)
            {
                Thread.Sleep(50);
                diskInformation = diskInfo.NextValue();
            }
            diskInformation = (float) Math.Round(diskInformation, 0);
            return diskInformation.ToString();
        }

        public string GetNetworkInfoSend ()
        {
            var NetworkInstance = new PerformanceCounterCategory("Network Interface");
            string instance = NetworkInstance.GetInstanceNames()[0];
            var networkInfo = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            var networkSend = networkInfo.NextValue();
            while (networkSend == 0)
            {
                Thread.Sleep(50);
                networkSend = networkInfo.NextValue();
            }
            networkSend = (float) Math.Round(((networkSend /1024) * 8), 0); // /2014 = KB, * 8 = kb
            return networkSend.ToString();
        }

        public string GetNetworkInfoReceived()
        {
            var NetworkInstance = new PerformanceCounterCategory("Network Interface");
            string instance = NetworkInstance.GetInstanceNames()[0];
            var networkInfo = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            var networkReceived = networkInfo.NextValue();
            while (networkReceived  == 0)
            {
                Thread.Sleep(50);
                networkReceived = networkInfo.NextValue();
            }
            networkReceived = (float) Math.Round(((networkReceived / 1024) * 8) / 1000, 1); /// 1024 = KB, * 8 = kb, / 1000 = mb
            return networkReceived.ToString();
        }

        private void updateInfo_Click(object sender, RoutedEventArgs e)
        {
            cpuInformation.Text = GetCpuInformation() + " %";
            ramInformation.Text = GetRamInformation() + " GB";
            diskInfo.Text = GetDiskActive() + " %";
            networkInfoSend.Text = GetNetworkInfoSend() + " Kb/sec";
            networkInfoReceived.Text = GetNetworkInfoReceived() + " Mb/sec";
        }
    }
}
