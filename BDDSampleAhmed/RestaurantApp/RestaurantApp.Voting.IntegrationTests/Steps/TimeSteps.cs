using System;
using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.IntegrationTests.Steps
{
    [Binding]
    public class TimeSteps
    {

        protected VotingTestContext VotingTestContext { get; set; }
        public TimeSteps(VotingTestContext context)
        {
            VotingTestContext = context;
        }

        [Given(@"que hoje é '(.*)'")]
        public void DadoQueAVotaçãoJaEstaEncerrada(DateTime data)
        {
            VotingTestContext.DateTimeProvider.Now = data;
        }
    }
}
