using Previsao.Temp.Lab.ViewModels;
using Previsao.Temp.Lab.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Previsao.Temp.Lab
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            var content = new BuscaCidadeView();
            content.BindingContext = new BuscaCidadesViewModel();

            MainPage = new NavigationPage(content);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
