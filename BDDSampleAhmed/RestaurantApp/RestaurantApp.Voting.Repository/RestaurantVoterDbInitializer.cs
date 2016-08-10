using RestaurantApp.Voting.Common.Models.Domain;
using System.Data.Entity;
using System.Linq;

namespace RestaurantApp.Voting.Repository
{
    public class RestaurantVoterDbInitializer : DropCreateDatabaseIfModelChanges<RestaurantVoterDbContext>
    {
        //CreateDatabaseIfNotExists<RestaurantVoterDbContext>
        protected override void Seed(RestaurantVoterDbContext context)
        {
            var users = new[] { "Ahmed", "Alejo", "Aderopo", "Rahman", "Rahim" }.Select(_ => new User { Name = _, Password = _ });
            var restaurantes = 
                new[] { "Piazza", "Pallato", "Banquette", "Ponto do Sabor", "Sabor do Porto", "Bom de mais" }
                .Select(_ => new Restaurant { Name = _ });

            context.Voters.AddRange(users);
            context.Restaurants.AddRange(restaurantes);

            //All standards will
            base.Seed(context);
        }
    }
}
