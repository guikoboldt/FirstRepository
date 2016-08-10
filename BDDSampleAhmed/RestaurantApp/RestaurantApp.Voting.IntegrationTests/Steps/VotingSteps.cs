using FluentAssertions;
using RestaurantApp.Voting.Business;
using RestaurantApp.Voting.Common.Models.Domain;
using RestaurantApp.Voting.Common.Models.Exceptions;
using System;
using System.Collections.Specialized;
using System.Linq;
using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.IntegrationTests.Steps
{
    [Binding]
    public class VotingSteps
    {

        protected VotingTestContext VotingTestContext { get; set; }
        public VotingSteps(VotingTestContext context)
        {
            VotingTestContext = context;
        }

        [Given(@"que o '(\w+)' votou no restaurante '(\w+)' '(\w+)'")]
        [Given(@"que o '(\w+)' votou no restaurante '(\w+)' na data '(\w+)")]
        [When(@"o '(\w+)' votou no restaurante '(\w+)' '(\w+)'")]
        [When(@"o '(\w+)' votou no restaurante '(\w+)' na data '(\w+)")]
        [Then(@"o '(\w+)' deveria poder votar no restaurante '(\w+)' '(\w+)'")]
        [Then(@"o '(\w+)' deveria poder votar no restaurante '(\w+)' na data '(\w+)'")]
        public void DadoQueOProfissionalVotouNoRestauranteNaData(string professional, string restaurante, DateTime date)
        {
            var previousDate = VotingTestContext.DateTimeProvider.Now;
            VotingTestContext.DateTimeProvider.Now = date;

            VotingTestContext.PollBO.VoteAsync(VotingTestContext.Db,
                new Vote { VoterName = professional, CandidateName = restaurante }).Wait();

            VotingTestContext.DateTimeProvider.Now = previousDate;
        }

        [Given(@"que '(.+)' votaram no restaurante '(\w+)' '(.+)'")]
        [Given(@"que '(.+)' votaram no restaurante '(\w+)' na data '(.+)'")]
        [Then(@"'(.+)' deveriam poder votar no restaurante '(\w+)' '(.+)'")]
        [Then(@"'(.+)' deveriam poder votar no restaurante '(\w+)' na data '(.+)'")]
        public void DadoQueOsProfissionaisVotouNoRestauranteNaData(StringCollection professionais, string restaurante, DateTime date)
        {
            foreach (var professional in professionais)
                DadoQueOProfissionalVotouNoRestauranteNaData(professional, restaurante, date);
        }

        [Given(@"que o '(\w+)' não pude votar no restaurante '(\w+)' '(.+)'")]
        [Given(@"que o '(\w+)' não pude votar no restaurante '(\w+)' na data '(.+)' denovo")]
        [Given(@"que o '(\w+)' não pude votar no restaurante '(\w+)' na data '(.+)' novamente")]
        [Then(@"o '(\w+)' não deveria poder votar no restaurante '(\w+)' '(.+)'")]
        [Then(@"o '(\w+)' não deveria poder votar no restaurante '(\w+)' '(.+)' denovo")]
        [Then(@"o '(\w+)' não deveria poder votar no restaurante '(\w+)' '(.+)' novamente")]
        [Then(@"o '(\w+)' não deveria poder votar no restaurante '(\w+)' na data '(.+)' denovo")]
        [Then(@"o '(\w+)' não deveria poder votar no restaurante '(\w+)' na data '(.+)' novamente")]
        public void EntaoOProfissionalNaoDeveriaPoderVotarNoRestauranteNaDataNovamente(string professional, string restaurante, DateTime date)
        {
            this.Invoking(_ => _.DadoQueOProfissionalVotouNoRestauranteNaData(professional, restaurante, date))
                .ShouldThrow<VoteRepetitionException>();
        }

        [Then(@"os restaurantes elegíveis '(\w+)' deveriam ser: (.*)")]
        public void EntaoOsRestauranteElegiveisDeveriamSer(DateTime date, StringCollection restaurantes)
        {                                                                  
            VotingTestContext.DateTimeProvider.Now = date;
            var candidates = VotingTestContext.PollBO
                                              .RetreiveElegibleCandidatesAsync(VotingTestContext.Db)
                                              .Result
                                              .Select(_ => _.Name);

            candidates.Should()
                      .HaveSameCount(restaurantes)
                      .And
                      .Contain(restaurantes);
        }

        [Then(@"os restaurantes mais votados deveriam ser: (.*)")]
        [Then(@"os restaurantes mais votados no momento deveriam ser: (.*)")]
        [Then(@"os restaurantes mais votados até o momento deveriam ser: (.*)")]
        public void EntaoOsRestaurantemaisVotadosDeveriamSer(StringCollection restaurantes)
        {
            var candidates = VotingTestContext.PollBO
                                              .RetreiveCandidatesAsync(VotingTestContext.Db, VotingTestContext.DateTimeProvider.Now.Date)
                                              .Result;

            var mostVotedCandidates = PollBuisnessObject.RetreiveMostVotedCandidates(candidates)
                                                        .Select(_ => _.Name);

            mostVotedCandidates.Should()
                               .HaveSameCount(restaurantes)
                               .And
                               .Contain(restaurantes);
        }

        [Then(@"o restaurante mais votado deveria ser: (.*)")]
        [Then(@"o restaurante mais votado no momento deveria ser: (.*)")]
        [Then(@"o restaurante mais votado até o momento deveria ser: (.*)")]
        public void EntaoOsRestaurantemaisVotadosDeveriamSer(string restaurante)
        {
            EntaoOsRestaurantemaisVotadosDeveriamSer(new StringCollection { restaurante });
        }

    }
}
