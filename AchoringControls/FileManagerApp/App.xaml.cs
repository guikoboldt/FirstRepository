﻿using FileManagerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileManagerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //ApplicationView app = new ApplicationView();
            //ApplicationViewModel context = new ApplicationViewModel();
            //app.DataContext = context;
            //app.Show();
            

            var main =  new Window
            {
                Content = new UserControls.MainWindowView(),
                Height = 600,
                Width = 800,
            };
            main.Show();
            
        }
    }
}
