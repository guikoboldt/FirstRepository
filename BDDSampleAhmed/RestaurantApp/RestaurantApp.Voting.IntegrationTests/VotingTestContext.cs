using RestaurantApp.Voting.Business;
using RestaurantApp.Voting.IntegrationTests.Helpers.Mocks;
using RestaurantApp.Voting.Repository;
using System;

namespace RestaurantApp.Voting.IntegrationTests
{
    /// <summary>
    /// Class usado para limpar o banco a cada
    /// </summary>
    public class VotingTestContext
    {
        private RestaurantVoterDbContext _Db;

        public VotingTestContext()
        {
            LaunchTime = DateTime.Now.TimeOfDay;
            DateTimeProvider = new TestDateTimeProvider { Now = DateTime.Now };
            PollBO = new PollBuisnessObject(DateTimeProvider, () => LaunchTime);
        }
        public TestDateTimeProvider DateTimeProvider { get; set; }

        public RestaurantVoterDbContext Db
        {
            get { return _Db = _Db ?? new RestaurantVoterDbContext(); }
        }

        public PollBuisnessObject PollBO { get; set; }
        public TimeSpan LaunchTime { get; set; }

        public UserAuthenticationBuisnessObject Authenticator { get; set; }

        public void BeforeStep()
        {
            AfterStep();
            _Db = new RestaurantVoterDbContext();
        }

        public void AfterStep()
        {
            if (_Db == null)
                return;

            _Db.Dispose();
        }
    }
}
