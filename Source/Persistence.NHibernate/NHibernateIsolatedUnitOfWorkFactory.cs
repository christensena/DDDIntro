using System;
using NHibernate;

namespace DDDIntro.Persistence.NHibernate
{
    public class NHibernateIsolatedUnitOfWorkFactory : IIsolatedUnitOfWorkFactory
    {
        private readonly ISessionFactory sessionFactory;

        public NHibernateIsolatedUnitOfWorkFactory(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
            this.sessionFactory = sessionFactory;
        }

        public IUnitOfWork BeginUnitOfWork()
        {
            return new NHibernateUnitOfWork(sessionFactory);
        }
    }
}