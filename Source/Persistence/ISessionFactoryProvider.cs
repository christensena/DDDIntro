using NHibernate;

namespace DDDIntro.Persistence
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}