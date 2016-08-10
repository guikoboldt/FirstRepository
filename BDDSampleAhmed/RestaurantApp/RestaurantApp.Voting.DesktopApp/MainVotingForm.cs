using RestaurantApp.Voting.Business;
using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.Common.Models.Exceptions;
using RestaurantApp.Voting.DesktopApp.Properties;
using RestaurantApp.Voting.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantApp.Voting.DesktopApp
{
    /// <summary>
    /// Representa interface de usuário principal para votação
    /// </summary>
    public partial class MainVotingForm : BaseForm
    {
        #region Properties
        /// <summary>
        /// O candidato(restaurante) selecionado
        /// </summary>
        public string SelectedCandidate
        {
            get { return (string)VoteOptionsListControl.SelectedValue; }
            set { VoteOptionsListControl.SelectedValue = value ?? string.Empty; }
        }

        /// <summary>
        /// get/set: indicado que voto já foi lançado
        /// </summary>
        public bool HasVoted { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }

        public PollBuisnessObject PollBO { get; set; }
        #endregion Properties

        public MainVotingForm(string voterName, PollBuisnessObject pollBO)
        {
            InitializeComponent();
            DateTimeProvider = pollBO.DateTimeProvider;
            LoggedUserNameLabel.Text = SubTitle = voterName;
            PollBO = pollBO;
        }

        #region EventHandlers
        private async void MainVotingForm_Load(object sender, EventArgs e)
        {
            //limita a data de consulta para hoje
            var today = DateTimeProvider.Now.Date;
            MonthCalendar.TodayDate =
            MonthCalendar.MaxDate = today;

            RefreshVoteResultButton.Visible = PollBO.IsLaunchTimeDue;

            LaunchTimeCountDownLabel.Visible =
            LaunchTimeCountDownTicker.Enabled = !PollBO.IsLaunchTimeDue;

            await LoadFormAsync(today);
        }
        private async void OnVote(object sender, EventArgs e)
        {
            await VoteAsync();

            ShowMessageFormat("'{0}' votado!", SelectedCandidate);
        }
        private async void OnPollDateChanged(object sender, DateRangeEventArgs e)
        {
            await LoadFormAsync(e.End.Date);
        }
        private async void OnRefresh(object sender, EventArgs e)
        {
            await LoadFormAsync(MonthCalendar.SelectionEnd);
        }
        private async void OnLaunchTimeCountDownTick(object sender, EventArgs e)
        {
            var time = DateTimeProvider.Now.TimeOfDay;
            if (PollBO.IsLaunchTimeDue)
                await OnLaunchtimeCountDownComplete();
            else
                OnLaunchTimeCountDownTick(time);
        }
        private async void OnAutoResultRefreshTimerTick(object sender, EventArgs e)
        {
            AutoResultRefreshTimer.Enabled = false;
            await LoadFormAsync(MonthCalendar.SelectionEnd);
        }
        #endregion EventHandlers

        #region LogicHandlers
        public void UIVoteRequirementsSatisfied()
        {
            if (!string.IsNullOrWhiteSpace(SelectedCandidate))
                return;

            throw new VotingException("Por favor selecione um Restaurante.");
        }
        public async Task VoteAsync()
        {
            UIVoteRequirementsSatisfied();

            using (var context = new RestaurantVoterDbContext())
            {
                var candidates = await PollBO.VoteAsync(context, new Vote { VoterName = SubTitle, CandidateName = SelectedCandidate });

                UpdateControls(candidates, DateTimeProvider.Now.Date);
            }

        }
        public async Task LoadFormAsync(DateTime date)
        {
            MostVotedCadidateLabel.Visible = false;

            using (var context = new RestaurantVoterDbContext())
            {
                var candidates = await PollBO.RetreiveCandidatesAsync(context, date);
                UpdateControls(candidates, date);
            }


        }
        public void UpdateControls(ICollection<Candidate> candidates, DateTime date)
        {
            var selectedCandidate = SelectedCandidate;

            var items = candidates.OrderByDescending(_ => _.Votes.Count)
                                  .ThenBy(_ => _.Name)
                                  .Select(_ => new { _.Name, Description = string.Format("{0} - {1} ", _.Votes.Count.ToString().PadLeft(2, ' '), _.Name) })
                                  .ToList();

            items.Insert(0, new { Name = string.Empty, Description = string.Empty });

            //restaura a opçao selecionada antes da votação.
            VoteOptionsListControl.DataSource = items;

            SelectedCandidate = selectedCandidate;

            var mostVotedCandidates = PollBuisnessObject.RetreiveMostVotedCandidates(candidates)
                                                        .ToList();

            HasVoted = candidates.Any(_ => _.Votes.Any(v => v.VoterName == SubTitle));

            MostVotedCadidateLabel.Text = Resources.Drawn;
            MostVotedCadidateLabel.Visible = true;

            if (mostVotedCandidates.Count == 1)
            {
                var mostVoted = mostVotedCandidates.Single();

                MostVotedCadidateLabel.Text =
                    string.Format("{0} {1} votos", mostVoted.Name, mostVoted.Votes.Count);
            }

            UpdateResultControls(date);
        }
        private void UpdateResultControls(DateTime selectedDate)
        {
            bool todaySelected = (selectedDate.Date == DateTimeProvider.Now.Date);

            VoteButton.Enabled = !HasVoted && todaySelected;
            PollClosingAlertLabel.Visible = !todaySelected;

            RefreshVoteResultButton.Visible = todaySelected && PollBO.IsLaunchTimeDue;

            AutoResultRefreshTimer.Enabled = todaySelected && !PollBO.IsLaunchTimeDue;
        }
        private void OnLaunchTimeCountDownTick(TimeSpan time)
        {
            LaunchTimeCountDownLabel.Text = (PollBO.LaunchTimeFactory() - time).ToString(Resources.hh_mm_ss_time_format);
        }
        private async Task OnLaunchtimeCountDownComplete()
        {
            LaunchTimeCountDownTicker.Enabled = false;

            ShowMessage("Horário de almoço!");

            await LoadFormAsync(MonthCalendar.SelectionEnd);

            LaunchTimeCountDownLabel.Visible = false;
        }
        #endregion LogicHandlers


    }
}
