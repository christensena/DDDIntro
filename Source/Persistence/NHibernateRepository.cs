using System;
using System.Linq;
using DDDIntro.Core;
using NHibernate;
using NHibernate.Linq;

namespace Persistence
{
    public class NHibernateRepository<TEntity> : IRepository<TEntity>
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
            session.Save(entity);
        }
    }
}