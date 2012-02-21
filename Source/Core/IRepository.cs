using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Core
{
    /// <summary>
    /// Simple traditional repository. 
    /// May go the way of the dodo if query objects takes off here.
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        TEntity GetById(int id);

        IQueryable<TEntity> FindAll();

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}