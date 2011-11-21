namespace DDDIntro.Core
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork BeginUnitOfWork();
    }
}