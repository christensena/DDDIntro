using System;
using System.Linq;
using System.Collections.Generic;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class MatchPersistence : PersistenceTestSuiteBase
    {
        private ContextSetUpHelper helper;

        [SetUp]
        public void SetUp()
        {
            helper = new ContextSetUpHelper(UnitOfWorkFactory);
        }

        [Test]
        public void PersistSingleInningsWithTwoOvers_ShouldBeRetrievable()
        {
            // Arrange
            var team1 = helper.SetUpTeam("New Zealand");
            var team2 = helper.SetUpTeam("South Africa");

            // Act
            int matchID;
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = new Match(DateTime.Today, team1, team2);
                uow.Add(match);

                var team1FirstInnings = match.NewInnings(team1);
                var over = team1FirstInnings.NewOver(team1.Members.Last());

                var openingBatsman1 = team2.Members.First();
                var openingBatsman2 = team2.Members.ElementAt(1);

                over.RecordDelivery(openingBatsman1, 0);
                over.RecordDelivery(openingBatsman1, 1);
                over.RecordDelivery(openingBatsman2, 0);
                over.RecordDelivery(openingBatsman2, 4);
                over.RecordDelivery(openingBatsman2, 0);
                over.RecordDelivery(openingBatsman2, 0);

                over = team1FirstInnings.NewOver(team1.Members.ElementAt(1));
                over.RecordDelivery(openingBatsman1, 0);
                over.RecordDelivery(openingBatsman1, 0);
                over.RecordDelivery(openingBatsman1, 4);
                over.RecordDelivery(openingBatsman1, 1);
                over.RecordDelivery(openingBatsman2, 0);
                over.RecordDelivery(openingBatsman2, 0);

                uow.Complete();

                matchID = match.Id;
            }

            // Assert
            var retrievedMatch = GetRepository<Match>().GetById(matchID);
            retrievedMatch.Should().NotBeNull();
            retrievedMatch.Innings.Should().HaveCount(1);
            var teamInnings = retrievedMatch.Innings.Single();
            teamInnings.BattingTeam.Should().Be(team1);
            teamInnings.FieldingTeam.Should().Be(team2);
            teamInnings.Overs.Should().HaveCount(2);

            var firstOver = teamInnings.Overs.First();
            firstOver.Balls.Should().HaveCount(6);
        }
    }
}