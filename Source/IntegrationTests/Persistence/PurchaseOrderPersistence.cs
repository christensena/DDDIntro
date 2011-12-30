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
        private Product[] products;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // set up for ourselves a supplier
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                supplier = new Supplier {Name = "Me"};

                uow.Add(supplier);

                products = new[]
                               {
                                   new Product {Title = "Widget 1", SellPrice = 12.50m},
                                   new Product {Title = "Widget 2", SellPrice = 10.99m}
                               };

                foreach (var product in products)
                {
                    uow.Add(product);
                }

                uow.Complete();
            }
        }

        [Test]
        public void SavingAnOrder_ShouldSaveItsOrderLines()
        {
            int orderId;

            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                // Arrange
                var orderSupplier = uow.GetById<Supplier>(supplier.Id); // need to re-get as different session. NH will cache so no cost
                var order = new PurchaseOrder(GetNextOrderNumber(), orderSupplier);
                
                var orderLine1 = order.AddOrderLine(uow.GetById<Product>(products[0].Id));
                orderLine1.Quantity = 2;

                var orderLine2 = order.AddOrderLine(uow.GetById<Product>(products[1].Id));
                orderLine2.Quantity = 1;

                // Act
                uow.Add(order); // by setting up cascade update in NH mappings, I don't need to add orderlines explicitly
                // order is my aggregate root. order lines should not be accessed via the repository

                uow.Complete();

                orderId = order.Id;
            }

            // Assert
            var retrievedOrder = GetRepository<PurchaseOrder>().GetById(orderId);
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
                    uow.GetById<Product>(products[random.Next(products.Length - 1)].Id));
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

        private PurchaseOrderLine AddRandomOrderLineToOrder(IAggregateRepository repository, PurchaseOrder order)
        {
            var orderLine = order.AddOrderLine(repository.GetById<Product>(products[random.Next(products.Length-1)].Id));
            orderLine.Quantity = random.Next(5);
            return orderLine;
        }

        private string GetNextOrderNumber()
        {
            var orderNumber = lastOrderNumber + 1;

            lastOrderNumber = orderNumber;

            return orderNumber.ToString("0000");
        }
    }
}