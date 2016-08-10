using System;
using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.AcceptanceTests.Transforms
{
    [Binding]
    public class DateTimeTransforms
    {
        protected VotingTestContext VotingTestContext { get; set; }
        public DateTimeTransforms(VotingTestContext context)
        {
            VotingTestContext = context;
        }

        [StepArgumentTransformation(@"de hoje")]
        [StepArgumentTransformation(@"hoje")]
        public DateTime GetTodaysDate()
        {
            return VotingTestContext.DateTimeProvider.Now.Date;
        }

        [StepArgumentTransformation(@"ontem")]
        public DateTime GetYesterdaysDate()
        {
            return VotingTestContext.DateTimeProvider.Now.Date.AddDays(-1);
        }

        //[StepArgumentTransformation]
        public DateTime ConvertDate(string date)
        {
            return DateTime.Parse(date);
        }
    }
}
