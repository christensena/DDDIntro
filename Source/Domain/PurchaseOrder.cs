using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDIntro.Domain
{
    public class PurchaseOrder
    {
        private IList<PurchaseOrderLine> orderLines; // List makes sense here as order is important.

        public virtual int Id { get; private set; }

        public virtual Supplier Supplier { get; private set; }

        public virtual IEnumerable<PurchaseOrderLine> OrderLines
        {
            get
            {
                return orderLines.ToArray();
            }
        }

        // for NH
        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Supplier supplier)
        {
            if (supplier == null) throw new ArgumentNullException("supplier");
            Supplier = supplier;

            orderLines = new List<PurchaseOrderLine>();
        }

        public virtual PurchaseOrderLine AddOrderLine(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            var orderLine = new PurchaseOrderLine(this, product);
            orderLines.Add(orderLine);
            return orderLine;
        }

        public virtual void RemoveOrderLine(PurchaseOrderLine purchaseOrderLine)
        {
            if (purchaseOrderLine == null) throw new ArgumentNullException("purchaseOrderLine");

            if (!orderLines.Contains(purchaseOrderLine))
            {
                throw new InvalidOperationException("Orderline does not exist on this order");
            }

            orderLines.Remove(purchaseOrderLine);
        }

        public virtual decimal GetOrderTotal()
        {
            return orderLines.Sum(orderLine => orderLine.GetLineTotal());
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
            if (obj.GetType() != typeof(PurchaseOrderLine)) return false;
            return Equals((PurchaseOrderLine)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}