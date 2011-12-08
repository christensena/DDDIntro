using DDDIntro.Domain;
using DDDIntro.Domain.Services;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Services
{
    [TestFixture]
    public class PurchaseOrderServiceSuite : ServiceTestSuiteBase
    {
        private PurchaseOrderService service;
        private int supplierId;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // set up for ourselves a supplier
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                var supplier = new Supplier {Name = "Me"};

                uow.Add(supplier);

                uow.Complete();

                supplierId = supplier.Id;
            }
        }

        [SetUp]
        public void SetUp()
        {
            service = new PurchaseOrderService(UnitOfWorkFactory);
        }

        [Test]
        public void GetNewPurchaseOrder_CreatesUnfinalisedPurchaseOrderWithOrderNumber()
        {
            // Arrange

            // Act
            var purchaseOrder = service.GetNewPurchaseOrder(supplierId);

            // Assert
            purchaseOrder.OrderNumber.Should().NotBeNullOrEmpty();
            purchaseOrder.IsFinalised.Should().BeFalse();
        }
    }
}