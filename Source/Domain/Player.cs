using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Player : Entity
    {
        public virtual string FirstName { get; private set; }

        public virtual string LastName { get; private set; }

        public Player(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        // for NH rehydration only
        protected Player()
        {
        }

        public virtual string GetFullName()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        public override string ToString()
        {
            return GetFullName();
        }
    }
}