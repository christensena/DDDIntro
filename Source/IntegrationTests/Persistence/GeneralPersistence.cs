using System;
using System.Linq;
using System.Collections.Generic;
using DDDIntro.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    [TestFixture]
    public class GeneralPersistence : PersistenceTestSuiteBase
    {
        [Test]
        public void BeginUnitOfWork_ExceptionOccursInScope_ChangesNotSaved()
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

    }
}