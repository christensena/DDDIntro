using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using FluentAssertions;
using NHibernate;
using NUnit.Framework;
using Persistence;

namespace IntegrationTests.Persistence
{
    [TestFixture]
    public class SupplierPersistence
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private ISessionFactory sessionFactory;
        private IRepository<Supplier> supplierRepository;
        private ISession repositorySession;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var databaseConfiguration = NHibernateConfigurationProvider.GetTempDatabaseConfiguration();
            sessionFactory = new SessionFactoryProvider(databaseConfiguration).GetSessionFactory();
            unitOfWorkFactory = new NHibernateUnitOfWorkFactory(sessionFactory);
        }

        [SetUp]
        public void SetUp()
        {
            repositorySession = sessionFactory.OpenSession();
            supplierRepository = new NHibernateRepository<Supplier>(repositorySession);
        }

        [TearDown]
        public void TearDown()
        {
            repositorySession.Dispose();
        }

        [Test]
        public void AddNewSupplier_WhenRetrievedFromRepository_ShouldReturnSavedSupplier()
        {
            // Arrange
            var supplier = new Supplier {Name = "test"};

            // Act
            using (var unitOfWork = unitOfWorkFactory.BeginUnitOfWork())
            {
                unitOfWork.Add(supplier);

                unitOfWork.Complete();
            }

            // Assert
            supplierRepository.FindAll().Where(s => s.Name == supplier.Name).Should().NotBeEmpty();
        }
    }
}