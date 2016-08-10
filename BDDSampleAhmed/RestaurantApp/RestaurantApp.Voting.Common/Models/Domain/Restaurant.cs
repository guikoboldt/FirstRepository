using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Voting.Common.Models.Domain
{
    public class Restaurant
    {
        [Key]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{{Name='{0}'}}", Name);
        }
    }
}
