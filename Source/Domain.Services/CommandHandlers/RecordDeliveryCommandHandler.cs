using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain.Commands;

namespace DDDIntro.Domain.Services.CommandHandlers
{
    public class RecordDeliveryCommandHandler : ICommandHandler<RecordDeliveryCommand>
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public RecordDeliveryCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void HandleCommand(RecordDeliveryCommand command)
        {
            using (var unitOfWork = unitOfWorkFactory.BeginUnitOfWork())
            {
                var match = unitOfWork.GetById<Match>(command.MatchId);
                if (match == null) throw new ArgumentException("Match not found with ID: " + command.MatchId);

                var batter = unitOfWork.GetById<Player>(command.BatterId);
                if (batter == null) throw new ArgumentException("Player not found with ID: " + command.BatterId);

                // yuck; should be able to just tell the Match to record the delivery
                match.Innings.Last().Overs.Last().RecordDelivery(batter, command.RunsScored);
                
                unitOfWork.Complete();
            }
        }
    }
}