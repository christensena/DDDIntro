namespace DDDIntro.Core
{
    // don't greatly like this; some persistence awareness leak
    public interface ISessionSharingUnitOfWorkFactory : IUnitOfWorkFactory
    {
    }
}