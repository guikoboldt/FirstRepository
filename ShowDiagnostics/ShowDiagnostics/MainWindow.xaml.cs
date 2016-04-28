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


namespace ShowDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ComputerInfo cp = new ComputerInfo();
        private double ramAvailableInfo;
        private double ramTotalInfo;
        private PerformanceCounter ramInfo = new PerformanceCounter("Memory", "Available MBytes");
        private PerformanceCounter cpuInfo = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter diskUsageInfo = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        private PerformanceCounter networkInfo;
        private Timer update = new Timer(2000);

        public MainWindow()
        {
            networkInfo = new PerformanceCounter("Network Interface", "Bytes Received/sec", CheckNetworkInterface()); //iniciate the network counter
            InitializeComponent();
            update.Enabled = true;
            update.Elapsed += (s, e) => { Application.Current.Dispatcher.Invoke(new System.Action(() => this.ShowInformation() )); }; //binding for showInformation method
            //update.Interval = 10000; //10000 ticks= 10s
            update.Start();
            //showInformation();
        }

        public void SaveLog()
        {
            // var path = "diagnosticLog.txt";
            var file = new System.IO.StreamWriter("diagnosticLog.txt"); //read the file and put it into file variable
            file.WriteLine(DateTime.Now);
            file.WriteLine("CPU :" + cpuInformation.Text);
            file.WriteLine("RAM :" + ramInformation.Text);
            file.WriteLine("Disk Usage :" + diskInformation.Text);
            file.WriteLine("Network Send :" + networkInfoSend.Text);
            file.WriteLine("Network Received  :" + networkInfoReceived.Text + "\n");
            file.Close();
        }

        public double GetRamInformation()
        {
            ramAvailableInfo = Math.Round(((Double)ramInfo.NextValue() / 1024), 2); //amount memory available in GB
            ramTotalInfo = Math.Round((((((Double)cp.TotalPhysicalMemory) / 1024) / 1024) / 1024), 2); //Amount memory installed in GB

            return (ramTotalInfo - ramAvailableInfo);
        }

        public double GetCpuInformation()
        {
            var cpuInformation = cpuInfo.NextValue();
            //while (cpuInformation == 0)
            //{
            //    Thread.Sleep(50); //miliseconds
            //    cpuInformation = cpuInfo.NextValue();
            //}
            cpuInformation = (float)Math.Round(cpuInformation, 0);
            return cpuInformation;
        }

        public double GetDiskActive()
        {
            var disUsageInformation = diskUsageInfo.NextValue();
            //while (diskInformation == 0)
            //{
            //    Thread.Sleep(50);
            //    diskInformation = diskInfo.NextValue();
            //}
            disUsageInformation = (float) Math.Round(disUsageInformation, 0);
            if (disUsageInformation > 100)
            { //sometimes the information get over than 100 %, it's normal when computer is too busy
                disUsageInformation = 100;
            }
                return disUsageInformation;
        }

        public double GetNetworkInfoSend()
        {
            //networkInfo = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            //var networkSend = networkInfo.NextValue();
            //while (networkSend == 0)
            //{
            //    Thread.Sleep(50);
            //    networkSend = networkInfo.NextValue();
            //}
            var networkSend = (float)Math.Round(((networkInfo.NextValue() / 1024) * 8), 0); // /2014 = KB, * 8 = kb
            return networkSend;
            //return instance;
        }

        public double GetNetworkInfoReceived()
        {
            //networkInfo = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            //var networkReceived = networkInfo.NextValue();
            //while (networkReceived == 0)
            //{
            //    Thread.Sleep(50);
            //    networkReceived = networkInfo.NextValue();
            //}
            var networkReceived = (float) Math.Round(((networkInfo.NextValue() / 1024) * 8), 0); /// 1024 = KB, * 8 = kb
            return networkReceived;
            //return instanceArray.ToString();
        }

        public string CheckNetworkInterface()
        {
            string instance = " ";
            var networkInstance = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            //var NetworkInstance = new PerformanceCounterCategory("Network Interface");
            //var instanceType = NetworkInstance.GetType();
            for (int i = 0; i < networkInstance.Length; i++)
            {
                if (networkInstance[i].OperationalStatus.ToString().Equals("Up") && (networkInstance[i].NetworkInterfaceType.ToString().Equals("Ethernet") || networkInstance[i].NetworkInterfaceType.ToString().StartsWith("Wireless")))
                {
                    instance = networkInstance[i].Description;
                    break;
                }
            }

            instance = instance.Replace("(", "[");
            instance = instance.Replace(")", "]");

            return instance;
        }

        public void ShowInformation()
        {
                // do whatever you want to do with shared object.
            cpuInformation.Text = GetCpuInformation().ToString() + " %";
            ramInformation.Text = GetRamInformation().ToString() + " GB";
            diskInformation.Text = GetDiskActive().ToString() + " %";
            networkInfoSend.Text = GetNetworkInfoSend().ToString() + " kb/sec";
            networkInfoReceived.Text = GetNetworkInfoReceived().ToString() + " kb/sec";
            SaveLog();
        }
    }
}
