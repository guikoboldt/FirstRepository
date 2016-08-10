using Infrastructure.Common;
using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.Common.Models.Exceptions;
using RestaurantApp.Voting.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Voting.Business
{
    /// <summary>
    /// Representa o objeto de regra de negócios de Votacoes
    /// </summary>
    public class PollBuisnessObject
    {
        /// <summary>
        ///Horário de encerramento da votação
        /// </summary>
        public Func<TimeSpan> LaunchTimeFactory { get; private set; }

        public IDateTimeProvider DateTimeProvider { get; private set; }

        /// <summary>
        /// get: Indicacao que dá para votar no momento
        /// </summary>
        public bool IsLaunchTimeDue
        {
            get { return DateTimeProvider.Now.TimeOfDay > LaunchTimeFactory().Subtract(TimeSpan.FromSeconds(1)); }
        }

        /// <summary>
        /// Cria um novo objeto de regra de negócio de votação
        /// especificando um provedor de hora 
        /// </summary>
        /// <param name="dateTimeProvider">provedor de horário</param>
        /// <param name="launchTimeFactoryFactory">Horário de almoço</param>
        public PollBuisnessObject(IDateTimeProvider dateTimeProvider, Func<TimeSpan> launchTimeFactoryFactory)
        {

            DateTimeProvider = dateTimeProvider;
            LaunchTimeFactory = launchTimeFactoryFactory;
        }


        public async Task<List<Poll>> RetreiveAllPolls(RestaurantVoterDbContext dbContext)
        {
            var query = from b in dbContext.Polls
                        orderby b.Date
                        select b;

            return await query.ToListAsync();
        }

        /// <summary>
        /// Vota em um candidato
        /// </summary>
        /// <param name="dbContext">objeto de acesso ao BD</param>
        /// <param name="vote"><see cref="Vote"/> a ser realizado</param>
        public async Task<ICollection<Candidate>> VoteAsync(RestaurantVoterDbContext dbContext, Vote vote)
        {
            if (dbContext == null
                || vote == null
                || string.IsNullOrWhiteSpace(vote.VoterName)
                || string.IsNullOrWhiteSpace(vote.CandidateName))
                throw new VotingException();

            var currentDateTime = DateTimeProvider.Now;

            vote.Candidate = null;
            vote.PollDate = currentDateTime;
            vote.Voter = null;

            var candidates = await RetreiveCandidatesAsync(dbContext, currentDateTime.Date, ensurePollExistence: true);

            var candidate = candidates.FirstOrDefault(_ => _.Name == vote.CandidateName);

            if (candidate == null)
                throw new NoElegibleRestaurantException();

            var alreadyVotedCandidate = candidates.Where(c => c.Votes.Any(v => v.VoterName == vote.VoterName))
                                                  .Select(_ => _.Name)
                                                  .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(alreadyVotedCandidate))
                throw new VoteRepetitionException(alreadyVotedCandidate);

            vote.Time = currentDateTime.TimeOfDay;

            candidate.Votes.Add(vote);

            await dbContext.SaveChangesAsync();

            return candidates;
        }

        /// <summary>
        /// Busca todos o candidatos elegível para a data fornecida.
        /// </summary>
        /// <param name="dbContext">objeto de acesso ao BD</param>
        /// <param name="pollDate">a data da votcao/eleicao</param>
        /// <param name="ensurePollExistence">
        /// indica se deve criar uma votação caso nao estiver criada.
        /// </param>
        /// <returns>Candidates for the poll specified</returns>
        public async Task<ICollection<Candidate>> RetreiveCandidatesAsync(RestaurantVoterDbContext dbContext, DateTime pollDate, bool ensurePollExistence = false)
        {
            var currentDate = pollDate.Date;

            var poll = await dbContext.Polls.FindAsync(currentDate);

            if (poll != null)
                return poll.Candidates;

            poll = new Poll { Date = currentDate };

            foreach (var candidate in await RetreiveElegibleCandidatesAsync(dbContext, currentDate))
                poll.Candidates.Add(candidate);

            if (!poll.Candidates.Any())
                throw new NoElegibleRestaurantException();

            if (ensurePollExistence)
            {
                dbContext.Polls.Add(poll);

                await dbContext.SaveChangesAsync();
            }

            return poll.Candidates;

        }


        /// <summary>
        /// Busca todos o candidatos elegível para a data fornecida.
        /// </summary>
        /// <param name="dbContext">objeto de acesso ao BD</param>
        /// <param name="date">a data da votcao/eleicao</param>
        /// <returns>Candidates for the poll specified</returns>
        private async Task<IList<Candidate>> RetreiveElegibleCandidatesAsync(RestaurantVoterDbContext dbContext, DateTime date)
        {
            var datePart = date.Date;
            var weekStart = datePart.GetStartOfWeekNow();

            var winnersOfTheWeek = (from p in dbContext.Polls
                                    from c in p.Candidates
                                    where p.Date >= weekStart && p.Date < date
                                          && c.Votes.Count == p.Candidates.Max(_ => _.Votes.Count)
                                    select c.Name);

            var candidates = await (from r in dbContext.Restaurants
                                    where !winnersOfTheWeek.Contains(r.Name)
                                    select new { r.Name, PollDate = date })
                            .ToListAsync();

            return candidates.ConvertAll(_ => new Candidate { Name = _.Name, PollDate = _.PollDate });
        }

        /// <summary>
        /// Busca todos o candidatos elegível para a data de hoje.
        /// </summary>
        /// <param name="dbContext">objeto de acesso ao BD</param>
        public async Task<IList<Candidate>> RetreiveElegibleCandidatesAsync(RestaurantVoterDbContext dbContext)
        {
            return await RetreiveElegibleCandidatesAsync(dbContext, DateTimeProvider.Now.Date);
        }

        /// <summary>
        /// Busca todos os usuários/eleitores
        /// </summary>
        /// <param name="dbContext">objeto de acesso ao BD</param>
        public async Task<IList<User>> RetreiveAllVotersAsync(RestaurantVoterDbContext dbContext)
        {
            return await dbContext.Voters.ToListAsync();
        }

        /// <summary>
        /// Buscar o(s) ganhador(es)/escolhido(s) do dia.
        /// </summary>
        /// <param name="candidates">candidatos a serem filtrados</param>
        /// <returns>os candidatos mais votados</returns>
        public static IEnumerable<Candidate> RetreiveMostVotedCandidates(IEnumerable<Candidate> candidates)
        {
            var candidatesList = candidates as List<Candidate> ?? candidates.ToList();

            if (!candidatesList.Any())
                return candidatesList;

            var maxVote = candidatesList.Max(_ => _.Votes.Count);

            return candidatesList.Where(_ => _.Votes.Count == maxVote);
        }
    }
}
