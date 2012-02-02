using System;

namespace DDDIntro.Domain
{
    public class PurchaseOrderLine
    {
        private int id; // no need to expose a getter as this is not an aggregate root. needed for identity only which we expose via Equals()

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
            Quantity = 1;
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
            return other.id == id;
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
            return id;
        }
    }
}