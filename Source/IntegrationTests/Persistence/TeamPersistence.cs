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

            var player1 = helper.SetUpPlayer("Ross", "Taylor");
            var player2 = helper.SetUpPlayer("Brendan", "MacCullum");
            var player12 = helper.SetUpPlayer("Jesse", "Ryder");

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

    }
}