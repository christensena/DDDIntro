using System;
using System.Linq;
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
            var nzl = helper.SetUpCountry("New Zealand");
            var zim = helper.SetUpCountry("Zimbabwe");

            var matchID = helper.SetUpMatch(nzl, zim);

            // Act
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = uow.GetById<Match>(matchID);

                var battingTeam = match.Team1;
                var bowlingTeam = match.Team2;

                var team1FirstInnings = match.NewInnings(battingTeam);

                var openingBowler1 = bowlingTeam.Members.Last();

                var over = team1FirstInnings.NewOver(openingBowler1);

                var openingBatsman1 = battingTeam.Members.First();
                var openingBatsman2 = battingTeam.Members.ElementAt(1);

                team1FirstInnings.CommenceBatterInnings(openingBatsman1);
                team1FirstInnings.CommenceBatterInnings(openingBatsman2);

                over.RecordDelivery(openingBatsman1, 0);
                over.RecordDelivery(openingBatsman1, 1);
                over.RecordDelivery(openingBatsman2, 0);
                over.RecordDelivery(openingBatsman2, 4);
                over.RecordDelivery(openingBatsman2, 0);
                over.RecordDelivery(openingBatsman2, 0);

                var openingBowler2 = bowlingTeam.Members.ElementAt(1);
                over = team1FirstInnings.NewOver(openingBowler2);
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
            retrievedMatch.Team1.Country.Should().Be(nzl);
            retrievedMatch.Team2.Country.Should().Be(zim);
            var teamInnings = retrievedMatch.Innings.Single();
            teamInnings.Overs.Should().HaveCount(2);

            var firstOver = teamInnings.Overs.First();
            firstOver.Balls.Should().HaveCount(6);
        }

        
    }
}