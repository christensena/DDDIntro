using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Player : EntityWithGeneratedId, IAggregateRoot
    {
        private string firstName;
        private string lastName;
        private Country country;

        public virtual string FirstName
        {
            get { return firstName; }
        }

        public virtual string LastName
        {
            get { return lastName; }
        }

        public virtual Country Country
        {
            get { return country; }
        }

        public Player(string firstName, string lastName, Country country)
        {
            if (country == null) throw new ArgumentNullException("country");
            this.firstName = firstName;
            this.lastName = lastName;
            this.country = country;
        }

        public virtual void ChangeCountry(Country country)
        {
            if (country == null) throw new ArgumentNullException("country");
            this.country = country;
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