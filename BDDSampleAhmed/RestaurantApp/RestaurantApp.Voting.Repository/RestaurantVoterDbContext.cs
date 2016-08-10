using RestaurantApp.Voting.Common.Models.Domain;
using System.Data.Entity;

namespace RestaurantApp.Voting.Repository
{
    public class RestaurantVoterDbContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Voters { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public RestaurantVoterDbContext()
            : base(typeof(RestaurantVoterDbContext).Name)
        {
        }
    }
}
