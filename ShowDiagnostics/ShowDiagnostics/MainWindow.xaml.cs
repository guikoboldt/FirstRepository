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
        private int countSave;
        private PerformanceCounter ramInfo = new PerformanceCounter("Memory", "Available MBytes");
        private PerformanceCounter cpuInfo = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter diskUsageInfo = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        private PerformanceCounter networkInfo;
        private Timer update = new Timer(1000); //new timer with 2s of interval

        public MainWindow()
        {
            networkInfo = new PerformanceCounter("Network Interface", "Bytes Received/sec", CheckNetworkInterface()); //iniciate the network counter
            InitializeComponent();
            update.Enabled = true; //enable timer
            update.Elapsed += (s, e) => { Application.Current.Dispatcher.Invoke(new System.Action(() => this.ShowInformation() )); }; //get the application owner to timer elapsed thread
            //update.Interval = 10000; //10000 ticks= 10s
            update.Start();
            //showInformation();
        }

        public void SaveLog()
        {
            // var path = "diagnosticLog.txt";
            using (var fs = new System.IO.FileStream("diagnosticLog.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write)) //read the file and put it into file variable
            using (var file = new System.IO.StreamWriter(fs))
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine("\nCPU :" + cpuInformation.Text);
                file.WriteLine("\nRAM :" + ramInformation.Text);
                file.WriteLine("\nDisk Usage :" + diskInformation.Text);
                file.WriteLine("\nNetwork Send :" + networkInfoSend.Text);
                file.WriteLine("\nNetwork Received  :" + networkInfoReceived.Text + "\n");
                file.Close();
            }
            
        }

        public double GetRamInformation()
        {
           var ramAvailableInfo = Math.Round(((Double)ramInfo.NextValue() / 1024), 2); //amount memory available in GB
           var ramTotalInfo = Math.Round((((((Double)cp.TotalPhysicalMemory) / 1024) / 1024) / 1024), 2); //Amount memory installed in GB

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
            cpuInformation.Text = GetCpuInformation().ToString() + " %";
            ramInformation.Text = GetRamInformation().ToString() + " GB";
            diskInformation.Text = GetDiskActive().ToString() + " %";
            networkInfoSend.Text = GetNetworkInfoSend().ToString() + " kb/sec";
            networkInfoReceived.Text = GetNetworkInfoReceived().ToString() + " kb/sec";
            countSave++;
            if (countSave % 10 == 0)
            {
                SaveLog();
            }
            
        }
    }
}
