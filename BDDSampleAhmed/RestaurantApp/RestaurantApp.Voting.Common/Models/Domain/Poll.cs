using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Voting.Common.Models.Domain
{
    public class Poll
    {
        public Poll()
        {
            Candidates = new Collection<Candidate>();
        }
        /// <summary>
        /// get/set: data da votação 
        /// </summary>
        [Key]
        public DateTime? Date { get; set; }
        /// <summary>
        ///  itens a serem votadas
        /// </summary>
        [InverseProperty("Poll")]
        public virtual ICollection<Candidate> Candidates { get; set; }
        public override string ToString()
        {
            return string.Format("{{Date='{0}', Candidates.Count='{1}'}}", Date, Candidates.Count);
        }
    }
}
