namespace DDDIntro.Domain
{
    public class Supplier
    {
        public virtual int Id { get; private set; }

        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}