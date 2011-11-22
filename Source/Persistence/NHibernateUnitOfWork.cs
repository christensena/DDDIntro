using System;
using System.Linq;
using DDDIntro.Core;
using NHibernate;
using NHibernate.Linq;

namespace Persistence
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly ITransaction transaction;

        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");

            session = sessionFactory.OpenSession();
            transaction = session.BeginTransaction();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            return session.Get<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return session.Query<TEntity>();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) throw new ArgumentNullException("entity");
            session.Save(entity);
        }

        public void Complete()
        {
            transaction.Commit();
        }

        public void Dispose()
        {
            if (transaction.IsActive)
                transaction.Rollback();

            session.Dispose();
        }
    }
}