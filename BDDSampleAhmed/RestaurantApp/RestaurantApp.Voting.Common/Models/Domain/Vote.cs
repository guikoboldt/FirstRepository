
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Voting.Common.Models.Domain
{
    public class Vote
    {
        /// <summary>
        /// get/set: Identificacao do candidato votado
        /// </summary>
        [Key, Column(Order = 1)]
        public string CandidateName { get; set; }
        /// <summary>
        /// get/set: Identificacao do eleitor
        /// </summary>
        [Key, Column(Order = 2)]
        public string VoterName { get; set; }
        /// <summary>
        /// get/set: data da votação 
        /// </summary>
        [Key, Column(Order = 3)]
        public DateTime PollDate { get; set; }
        /// <summary>
        /// get/set: Tempo da votação 
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// get/set: o eleitor
        /// </summary>
        [ForeignKey("VoterName")]
        public virtual User Voter { get; set; }
        /// <summary>
        /// get/set: Identificacao do candidato votado
        /// </summary>
        [ForeignKey("CandidateName,PollDate")]
        public virtual Candidate Candidate { get; set; }

        public override string ToString()
        {
            return string.Format("{{VoterName='{0}', CandidateName='{1}', PollDate='{2}', Time='{3}'}}", VoterName, CandidateName, PollDate, Time);
        }
    }
}
