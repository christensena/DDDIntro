using System.Linq;

namespace DDDIntro.Core
{
    public interface IAggregateRepository
    {
        TEntity GetById<TEntity>(int id) where TEntity : class;

        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}