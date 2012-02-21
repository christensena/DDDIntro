using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Country : Entity
    {
        public virtual string Name { get; private set; }

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