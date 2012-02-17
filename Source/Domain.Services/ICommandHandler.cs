using DDDIntro.Domain.Commands;

namespace DDDIntro.Domain.Services
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        void HandleCommand(TCommand command);
    }
}