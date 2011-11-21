using System;
using System.Linq;
using DDDIntro.Core;
using NHibernate;

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
            //return session.;
            return null;
        }

        public void Add(TEntity entity)
        {
            session.Save(entity);
        }
    }
}