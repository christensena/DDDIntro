using NHibernate;

namespace Persistence
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}