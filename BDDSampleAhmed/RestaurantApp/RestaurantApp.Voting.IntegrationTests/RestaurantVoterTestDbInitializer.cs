using System.Data.Entity;
using System.Linq;
using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.Repository;

namespace RestaurantApp.Voting.IntegrationTests
{
    public class RestaurantVoterTestDbInitializer : DropCreateDatabaseAlways<RestaurantVoterDbContext>
    {
    }
}
