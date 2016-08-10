using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.AcceptanceTests.Hooks
{
    [Binding]
    public class DatabaseHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        protected VotingTestContext VotingTestContext { get; set; }
        public DatabaseHooks(VotingTestContext context)
        {
            VotingTestContext = context;
        }

        [BeforeTestRun, AfterTestRun]
        public static void TestRunHook()
        {
            new VotingTestContext().Db.Database.Delete();
        }

        [BeforeScenario("requires_database")]
        public void BeforeDatabaseScenarios()
        {
            ClearDabase();
        }

        [BeforeStep("requires_database")]
        public void BeforeDatabaseSteps()
        {
            VotingTestContext.BeforeStep();
        }

        [AfterStep("requires_database")]
        public void AfterDatabaseSteps()
        {
            VotingTestContext.AfterStep();
        }
        private void ClearDabase()
        {
            VotingTestContext.Db.Votes.RemoveRange(VotingTestContext.Db.Votes);
            VotingTestContext.Db.SaveChanges();
            VotingTestContext.Db.Voters.RemoveRange(VotingTestContext.Db.Voters);
            VotingTestContext.Db.SaveChanges();
            VotingTestContext.Db.Candidates.RemoveRange(VotingTestContext.Db.Candidates);
            VotingTestContext.Db.SaveChanges();
            VotingTestContext.Db.Restaurants.RemoveRange(VotingTestContext.Db.Restaurants);
            VotingTestContext.Db.SaveChanges();
            VotingTestContext.Db.Polls.RemoveRange(VotingTestContext.Db.Polls);
            VotingTestContext.Db.SaveChanges();
        }
    }
}
