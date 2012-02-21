using System.Linq;
using DDDIntro.Domain;
using DDDIntro.Domain.Services.CommandHandlers;
using DDDIntro.Domain.Services.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Services.CommandHandling
{
    [TestFixture]
    public class DeliveryRecording : ServiceTestSuiteBase
    {
        private ContextSetUpHelper setUpHelper;

        [SetUp]
        public void SetUp()
        {
            setUpHelper = new ContextSetUpHelper(UnitOfWorkFactory);
        }

        [Test]
        public void HandleRecordDeliveryCommand_RunsScored_ShouldAddToAllScores()
        {
            // Arrange
            const int runsToBeScored = 2;

            var matchId = setUpHelper.SetUpMatch();

            StartFirstOver(matchId);

            var match = GetRepository<Match>().GetById(matchId);
            var command = new RecordDeliveryCommand(match.Id, runsToBeScored);

            // Act
            var commandHandler = new RecordDeliveryCommandHandler(UnitOfWorkFactory);
            commandHandler.HandleCommand(command);

            // Assert
            // let's just do a few; should really be unit tests
            var retrievedMatch = GetRepository<Match>().GetById(matchId);
            var retrievedCurrentTeamInnings = retrievedMatch.Innings.Last();
            retrievedCurrentTeamInnings.GetCurrentOver().RunsScored().Should().Be(runsToBeScored);
            retrievedCurrentTeamInnings.GetScore().Should().Be(runsToBeScored);

            //var retrievedBatterInnings = currentTeamInnings.BatterInnings.First(x => x.NotOut);
            //retrievedBatterInnings.RunsScored.Should().Be(runsToBeScored);
        }

        private void StartFirstOver(int matchId)
        {
            using (var unitOfWork = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var match = unitOfWork.GetById<Match>(matchId);

                var innings = match.NewInnings(match.Team1);

                innings.NewOver(match.Team2.Members.Last());

                var openingBatterInnings = innings.CommenceBatterInnings(match.Team1.Members.First());


                unitOfWork.Complete();
            }
        }
    }
}