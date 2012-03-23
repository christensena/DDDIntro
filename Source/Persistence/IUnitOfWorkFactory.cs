namespace DDDIntro.Persistence
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork BeginUnitOfWork();
    }
}