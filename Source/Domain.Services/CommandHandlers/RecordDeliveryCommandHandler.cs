using System;
using DDDIntro.Core;
using DDDIntro.Domain.Services.Commands;

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

                match.RecordDelivery(command.RunsScored);
                
                unitOfWork.Complete();
            }
        }
    }
}