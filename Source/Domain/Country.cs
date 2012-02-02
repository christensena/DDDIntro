namespace DDDIntro.Domain
{
    public class Country : Entity
    {
        public virtual string Name { get; private set; }

        public Country(string name)
        {
            Name = name;
        }

        // for NH rehydration only
        protected Country()
        {
        }
    }
}