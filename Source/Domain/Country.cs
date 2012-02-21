using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Country : EntityWithGeneratedId, IAggregateRoot
    {
        public virtual string Name { get; private set; }

        /// <summary>
        /// Use factory to create <see cref="Country"/> instances.
        /// </summary>
        internal Country(string name)
        {
            Name = name;
        }

        // for NH rehydration only
        protected Country()
        {
        }
    }
}