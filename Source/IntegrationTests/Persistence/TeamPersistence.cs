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
            var nzl = helper.SetUpCountry("New Zealand");
            var saf = helper.SetUpCountry("South Africa");

            //helper.PopulateCountryPlayerPool(country.Name);

            var rossTaylor = helper.SetUpPlayer("Ross", "Taylor", nzl.Name);
            var brendanMccullumn = helper.SetUpPlayer("Brendan", "Mccullum", nzl.Name);

            int matchID;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = new Match(DateTime.Today, nzl, saf);
                uow.Add(match);

                uow.Complete();

                matchID = match.Id;
            }

            // Act
            Player player1;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = uow.GetById<Match>(matchID);

                //helper.PickTeam(match.Team1, uow);
                var team1 = match.Team1;
                team1.AddMember(rossTaylor);
                team1.AddMember(brendanMccullumn);

                player1 = match.Team1.Members.First();

                uow.Complete();
            }

            // Assert
            var retrievedMatch = GetRepository<Match>().GetById(matchID);
            var retrievedTeam = retrievedMatch.Team1;
            retrievedTeam.Should().NotBeNull();
            retrievedTeam.Country.Should().Be(nzl);
            retrievedTeam.Members.ElementAt(0).Should().Be(player1);
        }

    }
}