using RestaurantApp.Voting.Business;
using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.DesktopApp.Config;
using RestaurantApp.Voting.Repository;
using System;
using System.Threading.Tasks;

namespace RestaurantApp.Voting.DesktopApp
{
    public partial class LoginForm : BaseForm
    {
        public string UserName
        {
            get { return UserOptionListControl.Text; }
        }

        public string Password
        {
            get { return UserPasswordTextInput.Text; }
        }

        public PollBuisnessObject PollBO { get; set; }
        public ConfigurationHandler ConfigHandler { get; set; }
        public LoginForm(IDateTimeProvider dateTimeProvider, ConfigurationHandler configHandler)
        {
            InitializeComponent();
            ConfigHandler = configHandler;
            PollBO = new PollBuisnessObject(dateTimeProvider, () => ConfigHandler.LaunchTime);
        }

        public virtual void OnExit()
        {
            Close();
        }

        private async Task LoginAsync()
        {
            LoginBotton.Enabled = false;
            try
            {
                if (!UILoginRequirementsSatisfied())
                    return;

                using (var dbContext = new RestaurantVoterDbContext())
                {
                    var authenticator = new UserAuthenticationBuisnessObject(dbContext);

                    await authenticator.AuthenticateAsync(new User { Name = UserName, Password = Password });
                }

                UserPasswordTextInput.Clear();

                ConfigHandler.Refresh();

                this.Hide();

                ShowVotingForm();

                this.Show();

            }
            finally
            {
                LoginBotton.Enabled = true;
            }
        }

        public virtual void ShowVotingForm()
        {
            new MainVotingForm(UserName, PollBO).ShowDialog();
        }

        public bool UILoginRequirementsSatisfied()
        {
            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(UserName))
                return true;

            ShowMessage("Por favor fornece seus credenciais.");
            return false;
        }
        private async void LoginForm_Load(object sender, System.EventArgs e)
        {
            await LoadFormAsync();
        }

        public async Task LoadFormAsync()
        {
            using (var storage = new RestaurantVoterDbContext())
            {
                var candidates = await PollBO.RetreiveAllVotersAsync(storage);

                UserOptionListControl.DataSource = candidates;
            }
        }
        private async void OnLogin(object sender, EventArgs e)
        {
            await LoginAsync();
        }

        private void OnExit(object sender, EventArgs e)
        {
            OnExit();
        }

    }
}
