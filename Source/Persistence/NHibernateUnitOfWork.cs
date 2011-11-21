using System;
using DDDIntro.Core;
using NHibernate;

namespace Persistence
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly ITransaction transaction;

        public NHibernateUnitOfWork(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;

            transaction = session.BeginTransaction();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new NHibernateRepository<TEntity>(session);
        }

        public void Complete()
        {
            transaction.Commit();
        }

        public void Dispose()
        {
            transaction.Rollback();
            session.Dispose();
        }
    }
}