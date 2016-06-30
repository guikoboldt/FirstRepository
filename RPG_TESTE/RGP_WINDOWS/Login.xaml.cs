using RGP_WINDOWS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RGP_WINDOWS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelLogin viewlogin = new ViewModelLogin();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewlogin;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            viewlogin.Password = passwordBox.Password;
        }
    }
}
