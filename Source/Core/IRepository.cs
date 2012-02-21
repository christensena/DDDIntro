using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Core
{
    /// <summary>
    /// Simple traditional repository. 
    /// May go the way of the dodo if query objects takes off here.
    /// </summary>
    /// <remarks>
    /// We're being DDD strict here; can't get at entities and value objects except
    /// by walking down from their aggregate root.
    /// </remarks>
    public interface IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        TEntity GetById(int id);

        IQueryable<TEntity> FindAll();

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}