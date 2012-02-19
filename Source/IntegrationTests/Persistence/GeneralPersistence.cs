using System;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class GeneralPersistence : PersistenceTestSuiteBase
    {
        [Test]
        public void BeginUnitOfWork_EntityAdded_ExceptionOccursInScope_EntityShouldNotBePersisted()
        {
            // Act
            try
            {
                using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
                {
                    uow.Add(new Country("Test"));

                    throw new Exception();
                }
            }
            catch (Exception)
            {
            }

            // Assert
            GetRepository<Country>().FindAll().Should().BeEmpty();
        }

        [Test]
        public void BeginUnitOfWork_NoExceptionsEntityAdded_EntityShouldBePersisted()
        {
            // Arrange

            // Act
            using (var uow = UnitOfWorkFactory.BeginUnitOfWork())
            {
                uow.Add(new Country("Test"));

                uow.Complete();
            }

            // Assert
            GetRepository<Country>().FindAll().Should().NotBeEmpty();
        }
    }
}