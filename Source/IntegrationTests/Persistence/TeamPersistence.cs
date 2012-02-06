using System;
using System.Linq;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class TeamPersistence : PersistenceTestSuiteBase
    {
        private ContextSetUpHelper helper;

        [SetUp]
        public void SetUp()
        {
            helper = new ContextSetUpHelper(UnitOfWorkFactory);
        }

        [Test]
        public void PersistingTeam_ShouldBeRetrievableForPlayersAndCountry()
        {
            // Arrange
            var country = helper.SetUpCountry("New Zealand");
            var otherCountry = helper.SetUpCountry("Other Country");

            var player1 = helper.SetUpPlayer("Ross", "Taylor", country.Name);
            var player2 = helper.SetUpPlayer("Brendan", "MacCullum", country.Name);
            var player12 = helper.SetUpPlayer("Jesse", "Ryder", country.Name);

            int matchID;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = new Match(DateTime.Today, country, otherCountry);
                uow.Add(match);

                uow.Complete();

                matchID = match.Id;
            }

            // Act
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = uow.GetById<Match>(matchID);

                var team = match.Team1;

                team.AddMember(player1);
                team.AddMember(player2);

                team.TwelfthMan = player12;

                uow.Add(team);

                uow.Complete();
            }

            // Assert
            var retrievedMatch = GetRepository<Match>().GetById(matchID);
            var retrievedTeam = retrievedMatch.Team1;
            retrievedTeam.Should().NotBeNull();
            retrievedTeam.Country.Should().Be(country);
            retrievedTeam.TwelfthMan.Should().Be(player12);
            retrievedTeam.Members.ElementAt(0).Should().Be(player1);
            retrievedTeam.Members.ElementAt(1).Should().Be(player2);
        }

    }
}