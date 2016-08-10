using RestaurantApp.Voting.Common.Models.Domain;
using System.Collections.Generic;

namespace RestaurantApp.Voting.Common.Models
{
    interface IVotable
    {
        /// <summary>
        /// Votos recebido
        /// </summary>
        ICollection<Vote> Votes { get; set; }
    }
}
