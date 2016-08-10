using RestaurantApp.Voting.Common.Models.Exceptions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RestaurantApp.Voting.DesktopApp
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            title = Text;
            InitializeComponent();
        }

        private string title;
        /// <summary>
        /// get/set: Titulo do Form
        /// </summary>
        [Browsable(true)]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (!string.IsNullOrWhiteSpace(title))
                    updateWindowText();
            }
        }

        private string subTitle;
        public string SubTitle
        {
            get { return subTitle; }
            set
            {
                subTitle = value;
                if (!string.IsNullOrWhiteSpace(subTitle)) 
                    updateWindowText();
            }
        }

        protected void updateWindowText()
        {
            Text = Title;

            if (!string.IsNullOrWhiteSpace(SubTitle))
                Text = string.Format("{0} - {1}", Title, SubTitle);
        }
        private void OnAboutClick(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        public static DialogResult ShowMessage(string message, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return MessageBox.Show(message, title, buttons, icon);
        }

        public static DialogResult ShowMessageFormat(string message, params object[] args)
        {
            return ShowMessage(string.Format(message, args));
        }

        public static DialogResult ShowErrorMessage(string message, params object[] args)
        {
            return ShowMessage(string.Format(message, args), icon: MessageBoxIcon.Error);
        }

        public static DialogResult ShowErrorMessage<T>(T exception, string message = "", params object[] args)
            where T : VotingException
        {
            if (exception == null) throw new ArgumentNullException("exception");

            var msg = string.IsNullOrWhiteSpace(message)
                ? exception.Message
                : string.Format("{0}:\n\n{1}", string.Format(message, args), exception.Message);

            return ShowMessage(msg, icon: MessageBoxIcon.Error);
        }

        public static DialogResult ShowUnhandledErrorMessage(
            Exception exception, string title = "Erro de Aplicação",
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            var msg = string.Format("{0} :\n\n{1}\n\nStack Trace:\n{2}",
                "Houve um erro de aplicação. Por favor entre em contato com o administrador " +
                "com seguinte informação.",
                exception.Message, exception.StackTrace);

            return ShowMessage(msg, title, buttons, MessageBoxIcon.Stop);
        }
    }
}
