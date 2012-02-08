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

            helper.PopulateCountryPlayerPool(country.Name);

            int matchID;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = new Match(DateTime.Today, country, otherCountry);
                uow.Add(match);

                uow.Complete();

                matchID = match.Id;
            }

            // Act
            Player player1;
            Player twelfthMan;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = uow.GetById<Match>(matchID);

                helper.PickTeam(match.Team1, uow);

                player1 = match.Team1.Members.First();
                twelfthMan = match.Team1.TwelfthMan;

                uow.Complete();
            }

            // Assert
            var retrievedMatch = GetRepository<Match>().GetById(matchID);
            var retrievedTeam = retrievedMatch.Team1;
            retrievedTeam.Should().NotBeNull();
            retrievedTeam.Country.Should().Be(country);
            retrievedTeam.TwelfthMan.Should().Be(twelfthMan);
            retrievedTeam.Members.ElementAt(0).Should().Be(player1);
        }

    }
}