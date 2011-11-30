namespace DDDIntro.Domain
{
    // this is mapped as a component
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
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            Country = country;
        }
    }
}