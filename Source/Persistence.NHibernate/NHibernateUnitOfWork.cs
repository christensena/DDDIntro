using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain.Abstract;
using NHibernate;
using NHibernate.Linq;

namespace DDDIntro.Persistence.NHibernate
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly bool sessionWasProvided;
        private readonly ITransaction transaction;

        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");

            session = sessionFactory.OpenSession();
            transaction = session.BeginTransaction();
            sessionWasProvided = false;
        }

        public NHibernateUnitOfWork(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;
            transaction = session.BeginTransaction();
            sessionWasProvided = true;
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class, IAggregateRoot
        {
            return session.Get<TEntity>(id);
        }

        public IQueryable<TEntity> FindAll<TEntity>() where TEntity : class, IAggregateRoot
        {
            return session.Query<TEntity>();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot
        {
            if (entity == null) throw new ArgumentNullException("entity");
            session.Save(entity);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot
        {
            if (entity == null) throw new ArgumentNullException("entity");
            session.Delete(entity);
        }

        public void Complete()
        {
            transaction.Commit();
        }

        public void Dispose()
        {
            try
            {
                if (transaction.IsActive)
                    transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();

                if (!sessionWasProvided)
                {
                    session.Dispose();
                }
            }
        }
    }
}