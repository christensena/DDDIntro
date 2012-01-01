using System;
using DDDIntro.Core;
using NHibernate;

namespace DDDIntro.Persistence
{
    public class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionFactory sessionFactory;

        public NHibernateUnitOfWorkFactory(ISessionFactory sessionFactory)
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