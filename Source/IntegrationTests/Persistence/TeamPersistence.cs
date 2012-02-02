using System;
using System.Linq;
using System.Collections.Generic;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class TeamPersistence : PersistenceTestSuiteBase
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void PersistingTeam_ShouldBeRetrievableForPlayersAndCountry()
        {
            // Arrange
            var country = SetUpCountry();

            var player1 = SetUpPlayer("Ross", "Taylor");
            var player2 = SetUpPlayer("Brendan", "MacCullum");
            var player12 = SetUpPlayer("Jesse", "Ryder");

            // Act
            Team team;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                team = new Team(country);

                team.AddMember(player1);
                team.AddMember(player2);

                team.TwelfthMan = player12;

                uow.Add(team);

                uow.Complete();
            }

            // Assert
            var retrievedTeam = GetRepository<Team>().GetById(team.Id);
            retrievedTeam.Should().NotBeNull();
            retrievedTeam.Country.Should().Be(country);
            retrievedTeam.TwelfthMan.Should().Be(player12);
            retrievedTeam.Members.ElementAt(0).Should().Be(player1);
            retrievedTeam.Members.ElementAt(1).Should().Be(player2);
        }

        private Player SetUpPlayer(string firstName, string lastName)
        {
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var player = new Player(firstName, lastName);

                uow.Add(player);

                uow.Complete();

                return player;
            }
        }

        private Country SetUpCountry()
        {
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var country = new Country("New Zealand");

                uow.Add(country);

                uow.Complete();

                return country;
            }
        }
    }
}