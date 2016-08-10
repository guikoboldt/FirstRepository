
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Voting.Common.Models.Domain
{
    /// <summary>
    /// Representa uma cadidatura
    /// </summary>
    public class Candidate : IVotable
    {
        public Candidate()
        {
            Votes = new Collection<Vote>();
        }
        /// <summary>
        /// get/set: Nome do candidato
        /// </summary>
        [Key, Column(Order = 1)]
        public string Name { get; set; }

        [ForeignKey("Name")]
        public virtual Restaurant Restaurant { get; set; }
        /// <summary>
        /// get/set: Identificacao do Votação
        /// </summary>
        [Key, Column(Order = 2)]
        public DateTime PollDate { get; set; }
        /// <summary>
        /// get/set: Votação
        /// </summary>
        [ForeignKey("PollDate")]
        public virtual Poll Poll { get; set; }
        /// <summary>
        /// get/set: os votos no candidato
        /// </summary>
        public virtual ICollection<Vote> Votes { get; set; }

        public override string ToString()
        {
            return string.Format("{{Name='{0}', PollDate='{1}', Votes.Count='{2}'}}", Name, PollDate, Votes.Count);
        }
    }
}
