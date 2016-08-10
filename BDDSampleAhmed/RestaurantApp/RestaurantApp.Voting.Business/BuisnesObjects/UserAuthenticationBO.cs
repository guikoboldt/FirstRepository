using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.Common.Models.Exceptions;
using RestaurantApp.Voting.Repository;
using System.Threading.Tasks;

namespace RestaurantApp.Voting.Business
{
    /// <summary>
    /// Representa o objeto de regra de negócios de authenticacao para votação
    /// </summary>
    public class UserAuthenticationBuisnessObject
    {
        public RestaurantVoterDbContext Db { get; set; }

        public UserAuthenticationBuisnessObject(RestaurantVoterDbContext context)
        {
            Db = context;
        }

        public async Task AuthenticateAsync(User user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.Name)
                || string.IsNullOrWhiteSpace(user.Password))
                throw new AuthenticationException();

            var existentUser = await Db.Voters.FindAsync(user.Name);

            if (existentUser == null || existentUser.Password != user.Password)
                throw new AuthenticationException();

        }
    }
}
