using RestaurantApp.Voting.Business;
using RestaurantApp.Voting.Common.Models.Exceptions;
using RestaurantApp.Voting.DesktopApp.Config;
using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace RestaurantApp.Voting.DesktopApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += OnApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);


            Application.Run(new LoginForm(new DateTimeProvider(), new ConfigurationHandler()));
        }

        /// <summary>
        /// Fecha a aplicação após um erro não tratada foi lançada, exibindo um mensagem de aviso
        /// para o usuário.
        /// </summary>
        /// <param name="sender">Origem do erro</param>
        /// <param name="args"></param>
        private static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            BaseForm.ShowUnhandledErrorMessage(args.ExceptionObject as Exception);

            Application.Exit();
        }

        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs args)
        {
            if (args.Exception is VotingException)
            {
                BaseForm.ShowErrorMessage(args.Exception as VotingException);
                return;
            }

            var result = BaseForm.ShowUnhandledErrorMessage(args.Exception, buttons: MessageBoxButtons.AbortRetryIgnore);

            if (result == DialogResult.Abort)
                Application.Exit();
        }
    }
}
