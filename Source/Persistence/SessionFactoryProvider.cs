using System;
using NHibernate;
using NHibernate.Cfg;

namespace Persistence
{
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        private readonly Configuration configuration;
        private ISessionFactory sessionFactory;

        public SessionFactoryProvider(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            this.configuration = configuration;
        }

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory ?? (sessionFactory = configuration.BuildSessionFactory());
        }
    }
}