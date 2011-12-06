using System.Linq;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests.Persistence
{
    [TestFixture]
    public class SupplierPersistence : PersistenceTestSuiteBase
    {
        [Test]
        public void AddNewSupplier_WhenRetrievedFromRepository_ShouldReturnSavedSupplier()
        {
            // Arrange
            var supplier = new Supplier
                               {
                                   Name = "test",
                                   Address = new Address("123 Anystreet St", "", "", "Christchurch", "New Zealand")
                               };

            // Act
            using (var unitOfWork = UnitOfWorkFactory.BeginUnitOfWork())
            {
                unitOfWork.Add(supplier);

                unitOfWork.Complete();
            }

            // Assert
            var retrievedSupplier = GetRepository<Supplier>().FindAll().Where(s => s.Name == supplier.Name).FirstOrDefault();
            retrievedSupplier.Should().NotBeNull();
            retrievedSupplier.Id.Should().NotBe(0);
            retrievedSupplier.Address.ShouldHave().AllProperties().EqualTo(supplier.Address);
        }
    }
}