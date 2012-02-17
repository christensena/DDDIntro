namespace DDDIntro.Domain.Commands
{
    public class RecordDeliveryCommand : ICommand
    {
        public int MatchId { get; private set; }

        public int BatterId { get; private set; }

        public int RunsScored { get; private set; }

        public RecordDeliveryCommand(
            int matchId,
            int batterId,
            int runsScored)
        {
            MatchId = matchId;
            BatterId = batterId;
            RunsScored = runsScored;
        }
    }
}