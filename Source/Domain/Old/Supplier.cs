namespace DDDIntro.Domain
{
    // this is an entity. Id field is provided
    public class Supplier
    {
        // private setter because we do not want to be able to assign an id
        // certainly it should be immutable!
        public virtual int Id { get; private set; }

        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        // this simple implementation of Equals is based on Id always being populated
        // if we have id assigned by the database then we only have one if the object is persisted
        public virtual bool Equals(Supplier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Supplier)) return false;
            return Equals((Supplier) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}