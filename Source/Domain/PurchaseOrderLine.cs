using System;

namespace DDDIntro.Domain
{
    public class PurchaseOrderLine
    {
        public virtual int Id { get; private set; }

        public virtual PurchaseOrder Order { get; private set; }

        public virtual Product Product { get; private set; } // if we change the product, we might as well replace the whole orderline? style choice perhaps

        public virtual int Quantity { get; set; }

        protected PurchaseOrderLine()
        {
            // for nhibernate to re-hydrate
        }

        // for the Order.AddOrderLine()
        internal PurchaseOrderLine(PurchaseOrder purchaseOrder, Product product)
        {
            if (purchaseOrder == null) throw new ArgumentNullException("purchaseOrder");
            if (product == null) throw new ArgumentNullException("product");
            Order = purchaseOrder;
            Product = product;
        }

        public virtual decimal GetLineTotal()
        {
            // because we made Product mandatory through the constructor we don't need to check for null
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