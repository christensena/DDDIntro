using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Core
{
    public interface IUniversalRepository
    {
        TEntity GetById<TEntity>(int id) where TEntity : class, IAggregateRoot;

        IQueryable<TEntity> FindAll<TEntity>() where TEntity : class, IAggregateRoot;

        void Add<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot;

        void Remove<TEntity>(TEntity entity) where TEntity : class, IAggregateRoot;
    }
}