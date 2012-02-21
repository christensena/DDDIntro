using AutoMapper;
using DDDIntro.Web.Infrastructure;
using NUnit.Framework;

namespace DDDIntro.UnitTests.Web.Infrastructure
{
    [TestFixture]
    public class AutoMapperConfigurationVerifier
    {
        [Test]
        public void AutoMapperConfiguration_Configure_AllConfiguredMappingsWork()
        {
            // Act
            AutoMapperConfiguration.Configure();

            // Assert
            Mapper.AssertConfigurationIsValid();
        }

    }
}