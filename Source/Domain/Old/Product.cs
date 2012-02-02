namespace DDDIntro.Domain
{
    public class Product
    {
        public virtual int Id { get; private set; }

        public virtual string Title { get; set; }

        public virtual decimal SellPrice { get; set; }
    }
}