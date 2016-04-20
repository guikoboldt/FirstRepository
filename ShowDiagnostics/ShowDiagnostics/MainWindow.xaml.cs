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
            PerformanceCounter cpuInfo = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter ramInfo = new PerformanceCounter("Memory", "Available MBytes");

            //cpuInfo.CategoryName = "Processor";
            //cpuInfo.CounterName = "% Processor Time";
            //cpuInfo.InstanceName = "_Total";

            cpuInformation.Text = cpuInfo.NextValue().ToString();
            ramInformation.Text = ramInfo.NextValue().ToString();
        }
    }
}
