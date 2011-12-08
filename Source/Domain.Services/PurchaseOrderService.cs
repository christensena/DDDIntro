using System;
using System.Linq;
using DDDIntro.Core;

namespace DDDIntro.Domain.Services
{
    public class PurchaseOrderService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public PurchaseOrderService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public PurchaseOrder GetNewPurchaseOrder(int supplierId)
        {
            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var supplier = uow.GetById<Supplier>(supplierId);
                if (supplier == null) throw new ArgumentException("Supplier not found with ID:" + supplierId);

                var orderNumber = GetNextOrderNumber(uow);

                var purchaseOrder = new PurchaseOrder(orderNumber, supplier);
                uow.Add(purchaseOrder);

                uow.Complete();

                return purchaseOrder;
            }
        }

        private static string GetNextOrderNumber(IAggregateRepository aggregateRepository)
        {
            // TODO: this is a naive implementation and would probably create clashes
            var lastOrderNumber = aggregateRepository.GetAll<PurchaseOrder>().Max(po => po.OrderNumber);
            if (string.IsNullOrEmpty(lastOrderNumber))
            {
                lastOrderNumber = "1";
            }

            var nextOrderNumber = (int.Parse(lastOrderNumber) + 1).ToString("0000");
            return nextOrderNumber;
        }
    }
}