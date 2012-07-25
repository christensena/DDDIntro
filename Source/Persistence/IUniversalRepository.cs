using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Persistence
{
    /// <summary>
    /// Simplified repository concept that allows me to query off
    /// any aggregate root. Really a simple abstraction over NHibernate session.
    /// May go away if I pursue query objects a bit more.
    /// </summary>
    public interface IUniversalRepository
    {
        TEntity GetById<TEntity>(int id) where TEntity : class, IAggregateRoot;

        IQueryable<TEntity> Query<TEntity>() where TEntity : class, IAggregateRoot;

        void Add<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot;

        void Remove<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot;
    }
}