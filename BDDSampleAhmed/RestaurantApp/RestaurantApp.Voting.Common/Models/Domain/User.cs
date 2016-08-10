using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Voting.Common.Models.Domain
{
    /// <summary>
    /// Representa um usuário/eleitor
    /// </summary>
    public class User
    {
        /// <summary>
        /// get/set: o nome do usuário
        /// </summary>
        [Key]
        public string Name { get; set; }
        /// <summary>
        /// get/set: senha do usuário
        /// </summary>
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format("{{Name='{0}', Password='{1}'}}", Name, Password);
        }
    }
}
