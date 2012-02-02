using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class PurchaseOrderPersistence : PersistenceTestSuiteBase
    {
        private readonly Random random = new Random();
        private int lastOrderNumber = 0;

        private Supplier supplier;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // set up for ourselves a supplier
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                supplier = new Supplier {Name = "Me"};

                uow.Add(supplier);

                uow.Complete();
            }
            
        }

        [Test]
        public void SavingAnOrder_ShouldSaveItsOrderLines()
        {
            PurchaseOrder originalOrder;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                // Arrange
                var orderSupplier = uow.GetById<Supplier>(supplier.Id); // need to re-get as different session. NH will cache so no cost
                originalOrder = new PurchaseOrder(GetNextOrderNumber(), orderSupplier);
                
                var orderLine1 = originalOrder.AddOrderLine(CreateAndPersistRandomProduct(uow));
                orderLine1.Quantity = 2;

                var orderLine2 = originalOrder.AddOrderLine(CreateAndPersistRandomProduct(uow));
                orderLine2.Quantity = 3;

                // Act
                uow.Add(originalOrder); // by setting up cascade update in NH mappings, I don't need to add orderlines explicitly
                // order is my aggregate root. order lines should not be accessed via the repository

                uow.Complete();
            }

            // Assert
            var retrievedOrder = GetRepository<PurchaseOrder>().GetById(originalOrder.Id);
            retrievedOrder.Should().NotBeNull();
            retrievedOrder.OrderLines.Should().HaveCount(2);
        }


        [Test]
        public void RemovingAnOrderLine_ShouldKeepRemainingLinesWhenRetrieved()
        {
            // Arrange
            var orderId = CreateOrderWithOrderLines(2);

            // Act
            PurchaseOrder originalOrder;
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                originalOrder = uow.GetById<PurchaseOrder>(orderId);

                originalOrder.RemoveOrderLine(originalOrder.OrderLines.First());

                uow.Complete();
            }

            // Assert
            var retrievedOrder = GetRepository<PurchaseOrder>().GetById(orderId);
            retrievedOrder.OrderLines.Should().BeEquivalentTo(originalOrder.OrderLines);
        }

        [Test]
        public void InsertingAnOrderLine_ShouldKeepOrderWhenRetrieved()
        {
            // Arrange
            var orderId = CreateOrderWithOrderLines(2);
            
            // Act
            PurchaseOrder originalOrder;
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                originalOrder = uow.GetById<PurchaseOrder>(orderId);

                var orderLine = originalOrder.InsertOrderLineAfter(
                    originalOrder.OrderLines.ElementAt(1), 
                    CreateAndPersistRandomProduct(uow));
                orderLine.Quantity = random.Next(5);
                
                uow.Complete();
            }

            // Assert
            var retrievedOrder = GetRepository<PurchaseOrder>().GetById(orderId);
            retrievedOrder.OrderLines.Should().BeEquivalentTo(originalOrder.OrderLines);
        }

        private int CreateOrderWithOrderLines(int count)
        {
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                // Arrange
                var orderSupplier = uow.GetById<Supplier>(supplier.Id); // need to re-get as different session. NH will cache so no cost
                var order = new PurchaseOrder(GetNextOrderNumber(), orderSupplier);

                for (var i = 0; i < count; i++)
                {
                    AddRandomOrderLineToOrder(uow, order);
                }

                // Act
                uow.Add(order); // by setting up cascade update in NH mappings, I don't need to add orderlines explicitly
                // order is my aggregate root. order lines should not be accessed via the repository

                uow.Complete();

                return order.Id;
            }            
        }

        private PurchaseOrderLine AddRandomOrderLineToOrder(IUnitOfWork uow, PurchaseOrder order)
        {
            var orderLine = order.AddOrderLine(CreateAndPersistRandomProduct(uow));
            orderLine.Quantity = random.Next(5);
            return orderLine;
        }

        private Product CreateAndPersistRandomProduct(IUnitOfWork uow)
        {
                var product = new Product { Title = "Widget 1", SellPrice = 12.50m };

                uow.Add(product);

            return product;
        }

        private string GetNextOrderNumber()
        {
            var orderNumber = lastOrderNumber + 1;

            lastOrderNumber = orderNumber;

            return orderNumber.ToString("0000");
        }
    }
}