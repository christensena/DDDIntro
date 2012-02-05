using System;
using System.Linq;
using DDDIntro.Core;
using NHibernate;
using NHibernate.Linq;

namespace DDDIntro.Persistence.NHibernate
{
    public class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ISession session;

        public NHibernateRepository(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;
        }

        public TEntity GetById(int id)
        {
            return session.Get<TEntity>(id);
        }

        public IQueryable<TEntity> FindAll()
        {
            return session.Query<TEntity>();
        }

        public void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            // not, not SaveOrUpdate as we don't need Update if we use Unit of Work semantics
            session.Save(entity);
        }

        public void Remove(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            session.Delete(entity);
        }
    }
}