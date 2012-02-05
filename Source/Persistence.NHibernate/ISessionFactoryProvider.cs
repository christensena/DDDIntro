using NHibernate;

namespace DDDIntro.Persistence.NHibernate
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}