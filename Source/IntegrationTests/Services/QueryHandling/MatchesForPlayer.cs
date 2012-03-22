using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using DDDIntro.Domain.Services;
using DDDIntro.Domain.Services.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Services.QueryHandling
{
    [TestFixture]
    public class MatchesForPlayer : ServiceTestSuiteBase
    {
        private IQueryHandler<MatchesForPlayerQuery, Match[]> queryHandler;
        private ContextSetUpHelper setUpHelper;

        [SetUp]
        public void SetUp()
        {
            queryHandler = Resolve<IQueryHandler<MatchesForPlayerQuery, Match[]>>();

            setUpHelper = new ContextSetUpHelper(UnitOfWorkFactory);
        }

        [Test]
        public void HandleMatchesForPlayerQuery_ShouldReturnMatchesPlayerWasMemberOf()
        {
            // Arrange
            var random = new Random();
            var nzl = setUpHelper.SetUpCountry("New Zealand");
            var aus = setUpHelper.SetUpCountry("Australia");

            setUpHelper.PopulateCountryPlayerPool(nzl.Name);
            setUpHelper.PopulateCountryPlayerPool(aus.Name);

            var matches = new List<Match>();

            for (var i = 0; i < 5 +random.Next(4); i++)
            {
                var matchID = setUpHelper.SetUpMatch(nzl, aus);
                var match = Resolve<IRepository<Match>>().GetById(matchID);
                matches.Add(match);
            }

            var randomMatch = matches.ElementAt(random.Next(matches.Count - 1));
            var selectedPlayerIndex = random.Next(randomMatch.Team1.Members.Count() - 1);
            var selectedPlayer = randomMatch.Team1.Members.ElementAt(selectedPlayerIndex);

            var expectedResult = matches.Where(m => m.Teams.Any(t => t.Members.Contains(selectedPlayer))).ToArray();

            // Act
            var result = queryHandler.Handle(new MatchesForPlayerQuery(selectedPlayer.Id));

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.First().Team1.Members.Should().BeEquivalentTo(expectedResult.First().Team1.Members);
        }
    }
}