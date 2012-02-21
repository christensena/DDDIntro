using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Core
{
    public interface IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        TEntity GetById(int id);

        IQueryable<TEntity> FindAll();

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}