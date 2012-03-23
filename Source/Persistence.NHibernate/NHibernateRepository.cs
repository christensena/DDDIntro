using System;
using System.Linq;
using DDDIntro.Domain.Abstract;
using NHibernate;
using NHibernate.Linq;

namespace DDDIntro.Persistence.NHibernate
{
    /// <remarks>
    /// If we do a GetById or FindAll() and make changes to anything in that aggregate
    /// then completing the request should save any changes made back to the db.
    /// If we new up an entity and Add it, it should get inserted back to the db.
    /// If we Delete an entity it should be deleted from the db.
    /// No need for Update() or Save() or Write() or nonsense like that!
    /// </remarks>
    public class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
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