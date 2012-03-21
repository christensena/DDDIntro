using DDDIntro.Core;
using DDDIntro.Domain;
using DDDIntro.Domain.Services.Queries;
using DDDIntro.Domain.Services.QueryHandlers;
using FluentAssertions;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Services.QueryHandling
{
    [TestFixture]
    public class PlayersForCountry : ServiceTestSuiteBase
    {
        private PlayersForCountryQueryHandler queryHandler;
        private ContextSetUpHelper setUpHelper;

        [SetUp]
        public void SetUp()
        {
            queryHandler = new PlayersForCountryQueryHandler(Resolve<IRepository<Player>>());

            setUpHelper = new ContextSetUpHelper(UnitOfWorkFactory);
        }

        [Test]
        public void HandleQuery_MixtureOfPlayersAndCountries_ReturnsOnlyPlayersFromCountrySpecified()
        {
            // Arrange
            var nzl = setUpHelper.SetUpCountry("New Zealand");
            var saf = setUpHelper.SetUpCountry("South Africa");

            setUpHelper.PopulateCountryPlayerPool(nzl.Name);
            setUpHelper.PopulateCountryPlayerPool(saf.Name);

            // Act
            var players = queryHandler.Handle(new PlayersForCountryQuery(nzl.Name));

            // Assert
            players.Should().OnlyContain(x => x.Country.Equals(nzl));
        }
    }
}