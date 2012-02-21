using System.Linq;

namespace DDDIntro.Core
{
    public interface IUniversalRepository
    {
        TEntity GetById<TEntity>(int id) where TEntity : class;

        IQueryable<TEntity> FindAll<TEntity>() where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}