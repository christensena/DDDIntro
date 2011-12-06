using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests.Persistence
{
    [TestFixture]
    public class PurchaseOrderPersistence : PersistenceTestSuiteBase
    {
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
                var order = new PurchaseOrder(uow.GetById<Supplier>(supplier.Id));
                var orderLine1 = order.AddOrderLine();
                orderLine1.Product = uow.GetById<Product>(products[0].Id); // note we can't just use the same instances

                var orderLine2 = order.AddOrderLine();
                orderLine2.Product = uow.GetById<Product>(products[1].Id);

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
    }
}