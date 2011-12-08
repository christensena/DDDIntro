using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDIntro.Domain
{
    public class PurchaseOrder
    {
        private IList<PurchaseOrderLine> orderLines; // List makes sense here as order is important.

        public virtual int Id { get; private set; } // immutable

        public virtual string OrderNumber { get; private set; } // immutable

        public virtual Supplier Supplier { get; private set; } // immutable

        public virtual IEnumerable<PurchaseOrderLine> OrderLines // read only; modify via controlled methods below
        {
            get
            {
                return orderLines.ToArray();
            }
        }

        // this is a mutable property but controlled through methods
        public virtual bool IsFinalised
        {
            get;
            private set;
        }

        // for NH
        protected PurchaseOrder()
        {
        }

        internal PurchaseOrder(string orderNumber, Supplier supplier)
        {
            if (string.IsNullOrEmpty(orderNumber)) throw new ArgumentNullException("orderNumber");
            if (supplier == null) throw new ArgumentNullException("supplier");
            OrderNumber = orderNumber;
            Supplier = supplier;

            orderLines = new List<PurchaseOrderLine>();
        }

        public virtual PurchaseOrderLine AddOrderLine(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");

            VerifyOrderCanBeModified();

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

            VerifyOrderCanBeModified();

            orderLines.Remove(purchaseOrderLine);
        }

        public virtual decimal GetOrderTotal()
        {
            return orderLines.Sum(orderLine => orderLine.GetLineTotal());
        }

        public virtual void Finalise()
        {
            if (IsFinalised)
            {
                throw new InvalidOperationException("Order has already been finalised");
            }
            
            // we would do any invariant checking here
            // to decide if the object is ready to be finalised
            // (if we had any we'd best expose an IsReadyToFinalise() method

            IsFinalised = true;
        }

        public virtual bool Equals(PurchaseOrder other)
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
            return Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        private void VerifyOrderCanBeModified()
        {
            if (IsFinalised)
            {
                throw new InvalidOperationException("Finalised orders cannot be modified");
            }
        }
    }
}