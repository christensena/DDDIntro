using System.Linq;

namespace DDDIntro.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);

        IQueryable<TEntity> FindAll();

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}