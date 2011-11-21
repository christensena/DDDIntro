using System.Linq;

namespace DDDIntro.Core
{
    public interface IRepository<TEntity>
    {
        TEntity GetById(int id);

        IQueryable<TEntity> FindAll();

        void Add(TEntity entity);
    }
}