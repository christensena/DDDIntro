using System;
using System.Linq;
using DDDIntro.Core;

namespace DDDIntro.Domain.Services.Factories
{
    public class CountryFactory : IEntityFactory
    {
        private readonly IRepository<Country> countryRepository;

        public CountryFactory(IRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Country CreateCountry(string name)
        {
            if (countryRepository.FindAll().Any(c => c.Name == name))
            {
                throw new ArgumentException("There is already a country with that name!");
            }

            return new Country(name);
        }
    }
}