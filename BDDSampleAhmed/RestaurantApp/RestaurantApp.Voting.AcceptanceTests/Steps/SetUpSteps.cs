using RestaurantApp.Voting.Common.Models.Domain;
using System.Collections.Specialized;
using System.Linq;
using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.AcceptanceTests.Steps
{
    [Binding]
    public class SetUpSteps
    {

        protected VotingTestContext VotingTestContext { get; set; }
        protected SetUpSteps(VotingTestContext context)
        {
            VotingTestContext = context;
        }

        [Given(@"os seguintes profissionais: (.+)")]
        public void DadosOsSeguintesProfissionais(StringCollection voterNames)
        {
            var voters = voterNames.Cast<string>()
                                   .Select(n => new User { Name = n, Password = n });

            VotingTestContext.Db.Voters.AddRange(voters);
            VotingTestContext.Db.SaveChanges();
        }

        [Given(@"os seguintes restaurantes: (.+)")]
        public void DadosOsSeguintesRestaurantes(StringCollection restaurantNames)
        {
            var restaurants = restaurantNames.Cast<string>()
                                             .Select(n => new Restaurant { Name = n });

            VotingTestContext.Db.Restaurants.AddRange(restaurants);
            VotingTestContext.Db.SaveChanges();
        }
    }
}
