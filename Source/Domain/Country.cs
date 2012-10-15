using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Country : EntityWithGeneratedId, IAggregateRoot
    {
        private string name;

        public virtual string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Use factory to create <see cref="Country"/> instances.
        /// </summary>
        internal Country(string name)
        {
            this.name = name;
        }

        // for NH rehydration only
        protected Country()
        {
        }
    }
}