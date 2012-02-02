using System;

namespace DDDIntro.Domain
{
    // this is mapped as a component. 
    // ends up just being fields on the table of a mapped entity but treated as a full class
    // immutable; to change it we need to replace it
    public class Address
    {
        public virtual string Line1 { get; private set; }

        public virtual string Line2 { get; private set; }

        public virtual string Line3 { get; private set; }

        public virtual string City { get; private set; }

        public virtual string Country { get; private set; }

        protected Address()
        {
            
        }

        public Address(string line1, string line2, string line3, string city, string country)
        {
            if (line1 == null) throw new ArgumentNullException("line1");
            if (line2 == null) throw new ArgumentNullException("line2");
            if (line3 == null) throw new ArgumentNullException("line3");
            if (city == null) throw new ArgumentNullException("city");
            if (country == null) throw new ArgumentNullException("country");
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            Country = country;
        }

        // Equals implementation generated using R#
        // all properties included as this is a Value Object (DDD)
        // i.e. if all properties are the same as another object (but different reference) then
        // they are considered the same
        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Line1, Line1) && Equals(other.Line2, Line2) && Equals(other.Line3, Line3) && Equals(other.City, City) && Equals(other.Country, Country);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Address)) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Line1.GetHashCode();
                result = (result*397) ^ Line2.GetHashCode();
                result = (result*397) ^ Line3.GetHashCode();
                result = (result*397) ^ City.GetHashCode();
                result = (result*397) ^ Country.GetHashCode();
                return result;
            }
        }
    }
}