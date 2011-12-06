using System;

namespace DDDIntro.Domain
{
    public class PurchaseOrderLine
    {
        public virtual int Id { get; private set; }

        public virtual PurchaseOrder Order { get; private set; }

        public virtual Product Product { get; set; }

        public virtual int Quantity { get; set; }

        protected PurchaseOrderLine()
        {
            // for nhibernate to re-hydrate
        }

        // for the Order.AddOrderLine()
        internal PurchaseOrderLine(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null) throw new ArgumentNullException("purchaseOrder");
            Order = purchaseOrder;
        }

        public virtual decimal GetLineTotal()
        {
            if (Product == null)
                return 0m; // what is best to do here is decided by domain modelling. what should this value be?

            return Quantity*Product.SellPrice;
        }

        public virtual bool Equals(PurchaseOrderLine other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PurchaseOrderLine)) return false;
            return Equals((PurchaseOrderLine) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}