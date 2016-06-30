using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShowDiagnostics.Utilitys
{
    class ComputerCounter
    {
        private PerformanceCounter ramInfo;
        private PerformanceCounter cpuInfo;
        private PerformanceCounter diskUsageInfo;
        private ComputerInfo cp = new ComputerInfo();
        private PerformanceCounter[] networkInfoSent;
        private PerformanceCounter[] networkInfoReceived;
        List <string> interfaces;
        private double cpuAverage;
        private double cpuMaximun;
        private double cpuMinimun;
        private double ramAverage;
        private double ramMaximun;
        private double ramMinimun;
        private double diskAverage;
        private double diskMaximun;
        private double diskMinimun;
        private double[] networkSentAverage;
        private double[] networkSentMaximun;
        private double[] networkSentMinimun;
        private double[] networkReceivedAverage;
        private double[] networkReceivedMaximun;
        private double[] networkReceivedMinimun;

        public ComputerCounter ()
        {
            ramInfo = new PerformanceCounter("Memory", "Available MBytes");
            cpuInfo = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            diskUsageInfo = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            networkInfoSent = new PerformanceCounter[CheckNetworkInterface().Count];
            networkInfoReceived = new PerformanceCounter[CheckNetworkInterface().Count];
            interfaces = CheckNetworkInterface();
            for (int i = 0; i < CheckNetworkInterface().Count; i++)
            {
                networkInfoSent[i] = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaces[i]); //iniciate the network counter
                networkInfoReceived[i] = new PerformanceCounter("Network Interface", "Bytes Received/sec", interfaces[i]); //iniciate the network counter
            }
            networkSentAverage = new double [CheckNetworkInterface().Count];
            networkSentMaximun = new double[CheckNetworkInterface().Count];
            networkSentMinimun = new double[CheckNetworkInterface().Count];
            networkReceivedAverage = new double[CheckNetworkInterface().Count];
            networkReceivedMaximun = new double[CheckNetworkInterface().Count];
            networkReceivedMinimun = new double[CheckNetworkInterface().Count];
            SetDefaultValues();
        }



        public List<string> CheckNetworkInterface()
        {
            var networkInstance = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            //string instance = " ";
            List<string> interfaces = new List<string>();
            //int countNetworkUp = 0;
            for (int i = 0; i < networkInstance.Length; i++)
            {
                if (networkInstance[i].OperationalStatus.ToString().Equals("Up") && !networkInstance[i].NetworkInterfaceType.ToString().Equals("Loopback"))
                {
                    interfaces.Add(networkInstance[i].Description.ToString());
                }
            }

            for (int i = 0; i < interfaces.Count; i++)
            {
                interfaces[i] = interfaces[i].Replace("(", "[");
                interfaces[i] = interfaces[i].Replace(")", "]");
            }
            return interfaces;
        }

        public void SetDefaultValues()
        {
            cpuMaximun = 0;
            cpuAverage = 0;
            cpuMinimun = -1;
            ramMaximun = 0;
            ramAverage = 0;
            ramMinimun = -1;
            diskMaximun = 0;
            diskAverage = 0;
            diskMinimun = -1;
            for (int i = 0; i < networkInfoSent.Length; i++)
            {
                networkSentAverage[i] = 0;
                networkSentMaximun[i] = 0;
                networkSentMinimun[i] = -1;
                networkReceivedAverage[i] = 0;
                networkReceivedMaximun[i] = 0;
                networkReceivedMinimun[i] = -1;
            }
        }

        public void CheckFileLog()
        {
            if (!System.IO.File.Exists("diagnosticLog.txt"))
            {
                using (var logWriter = new System.IO.StreamWriter("diagnosticLog.txt", false))
                {
                    logWriter.Write("date;cpuMaximun;cpuMinimun;cpuAverage;ramMaximun;ramMinimun;ramAverage;diskMaximun;diskMinimun;" +
                        "diskAverage;");
                    for (int i = 0; i < interfaces.Count; i++)
                    {
                        var formatedName = interfaces[i].Replace(" ", "");
                        logWriter.WriteLine(formatedName + "SentMaximun;" + formatedName + "SentMinimun;" + formatedName + "SentAverage;" + formatedName + "ReceivedMaximun;" + formatedName + "ReceivedMinimun;" + formatedName + "ReceivedAverage");
                    }
                }
            }
        }

        public void SaveLog(int logTime)
        {
            CheckFileLog();
            FormatValuesToLog(logTime);
            //logFile = new System.IO.FileStream("diagnosticLog.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write); //read the file and put it into file variable
            try
            {
                using (var fileLogWriter = new System.IO.StreamWriter("diagnosticLog.txt", true))
                {
                    fileLogWriter.Write (DateTime.Now + ";" +
                                         ToString());
                    fileLogWriter.WriteLine("");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("The file is in use by another application!");
            }
            SetDefaultValues();
        }

        public double GetRamInformation()
        {
            var ramAvailableInfo = Math.Round(((Double)ramInfo.NextValue() / 1024), 2); //amount memory available in GB
            var ramTotalInfo = Math.Round((((((Double)cp.TotalPhysicalMemory) / 1024) / 1024) / 1024), 2); //Amount memory installed in GB

            ramAverage += (ramTotalInfo - ramAvailableInfo);
            if (ramMinimun == -1)
            {
                ramMinimun = (ramTotalInfo - ramAvailableInfo);
            }
            else if ((ramTotalInfo - ramAvailableInfo) < ramMinimun)
            {
                ramMinimun = (ramTotalInfo - ramAvailableInfo);
            }
            else if ((ramTotalInfo - ramAvailableInfo) > ramMaximun)
            {
                ramMaximun = (ramTotalInfo - ramAvailableInfo);
            }


            return (ramTotalInfo - ramAvailableInfo);
        }

        public double GetCpuInformation()
        {
            var cpuInformation = cpuInfo.NextValue();

            cpuInformation = (float)Math.Round(cpuInformation, 0);
            cpuAverage += cpuInformation;
            if (cpuMinimun == -1)
            {
                cpuMinimun = cpuInformation;
            }
            else if (cpuInformation < cpuMinimun)
            {
                cpuMinimun = cpuInformation;
            }
            else if (cpuInformation > cpuMaximun)
            {
                cpuMaximun = cpuInformation;
            }


            return cpuInformation;
        }


        public double GetDiskActive()
        {
            var disUsageInformation = diskUsageInfo.NextValue();
            disUsageInformation = (float)Math.Round(disUsageInformation, 0);
            if (disUsageInformation > 100)
            { //sometimes the information get over than 100 %, it's normal when computer is too busy
                disUsageInformation = 100;
            }

            diskAverage += disUsageInformation;
            if (diskMinimun == -1)
            {
                diskMinimun = disUsageInformation;
            }
            else if (disUsageInformation < diskMinimun)
            {
                diskMinimun = disUsageInformation;
            }
            else if (disUsageInformation > diskMaximun)
            {
                diskMaximun = disUsageInformation;
            }

            return disUsageInformation;
        }

        public double[] GetNetworkInfoSent()
        {
            double[] networkSent = new double[networkInfoSent.Length];
            for (int i = 0; i < networkInfoSent.Length; i++)
            {
                networkSent[i] = (float)Math.Round(((networkInfoSent[i].NextValue() / 1024) * 8), 0); // /2014 = KB, * 8 = kb
                networkSentAverage[i] += networkSent[i];
                if (networkSentMinimun[i] == -1)
                {
                    networkSentMinimun[i] = networkSent[i];
                }
                else if (networkSent[i] < networkSentMinimun[i])
                {
                    networkSentMinimun[i] = networkSent[i];
                }
                else if (networkSent[i] > networkSentMaximun[i])
                {
                    networkSentMaximun[i] = networkSent[i];
                }
            }
            return networkSent;
        }

        public double[] GetNetworkInfoReceived ()
        {
            double[] networkReceived = new double [networkInfoReceived.Length];
            for (int i = 0; i < networkInfoReceived.Length; i++)
            {
                networkReceived[i] = (float)Math.Round(((networkInfoReceived[i].NextValue() / 1024) * 8), 0); /// 1024 = KB, * 8 = kb

                networkReceivedAverage[i] += networkReceived[i];
                if (networkReceivedMinimun[i] == -1)
                {
                    networkReceivedMinimun[i] = networkReceived[i];
                }
                else if (networkReceived[i] < networkReceivedMinimun[i])
                {
                    networkReceivedMinimun[i] = networkReceived[i];
                }
                else if (networkReceived[i] > networkReceivedMaximun[i])
                {
                    networkReceivedMaximun[i] = networkReceived[i];
                }
            }
            return networkReceived;
        }

        public void FormatValuesToLog(int logTime)
        {
            cpuAverage = Math.Round(cpuAverage / logTime, 2);
            ramAverage = Math.Round(ramAverage / logTime, 2);
            diskAverage = Math.Round(diskAverage / logTime, 2);
            for (int i = 0; i < networkInfoSent.Length; i++)
            {
                networkSentAverage[i] = Math.Round(networkSentAverage[i] / logTime, 2);
                networkReceivedAverage[i] = Math.Round(networkReceivedAverage[i] / logTime, 2);
            }
        }

        override
        public string ToString ()
        {
            string allInformation = cpuAverage + ";" + cpuMaximun + ";" + cpuMinimun + ";" +
                        ramAverage + ";" + ramMaximun + ";" + ramMinimun + ";" +
                         diskAverage + ";" + diskMaximun + ";" + diskMinimun;
            for (int i = 0; i < networkInfoSent.Length; i++)
            {
                allInformation = allInformation + ";" + networkSentAverage[i] + ";" + networkSentMaximun[i] + ";" + networkSentMinimun[i] + ";" +
                        networkReceivedAverage[i] + ";" + networkReceivedMaximun[i] + ";" + networkReceivedMinimun[i];
            }
            return allInformation;
        }

        public List<string> GetInterfaces ()
        {
            return this.interfaces;
        }

    }
}
