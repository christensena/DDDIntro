using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using DDDIntro.Domain.Services.Factories;

namespace DDDIntro.Application.Services
{
    public class TestDataGenerator
    {
        private readonly IIsolatedUnitOfWorkFactory unitOfWorkFactory;
        private readonly CountryFactory countryFactory;

        public TestDataGenerator(IIsolatedUnitOfWorkFactory unitOfWorkFactory, CountryFactory countryFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.countryFactory = countryFactory;
        }

        public void GenerateTestData()
        {
            GenerateCountries();
            GenerateTeams();
        }

        public void GenerateCountries()
        {
            using (var unitOfWork = unitOfWorkFactory.BeginUnitOfWork())
            {
                unitOfWork.Add(countryFactory.CreateCountry("New Zealand"));
                unitOfWork.Add(countryFactory.CreateCountry("Australia"));
                unitOfWork.Add(countryFactory.CreateCountry("South Africa"));
                unitOfWork.Add(countryFactory.CreateCountry("India"));
                unitOfWork.Add(countryFactory.CreateCountry("England"));
                unitOfWork.Add(countryFactory.CreateCountry("West Indies"));
                unitOfWork.Add(countryFactory.CreateCountry("Sri Lanka"));
                unitOfWork.Add(countryFactory.CreateCountry("Pakistan"));
                unitOfWork.Add(countryFactory.CreateCountry("Bangladesh"));
                unitOfWork.Add(countryFactory.CreateCountry("Zimbabwe"));

                unitOfWork.Complete();
            }
        }

        public void GenerateTeams()
        {
            using (var unitOfWork = unitOfWorkFactory.BeginUnitOfWork())
            {
                var nzl = unitOfWork.FindAll<Country>().SingleOrDefault(c => c.Name == "New Zealand");

                unitOfWork.Add(new Player("Brendan", "McCullum", nzl));
                unitOfWork.Add(new Player("Martin", "Guptil", nzl));
                unitOfWork.Add(new Player("Ross", "Taylor", nzl));
                unitOfWork.Add(new Player("Rob", "Nichol", nzl));
                unitOfWork.Add(new Player("Kane", "Williamson", nzl));
                unitOfWork.Add(new Player("Jacob", "Oram", nzl));
                unitOfWork.Add(new Player("Daniel", "Vettori", nzl));
                unitOfWork.Add(new Player("Tim", "Southee", nzl));
                unitOfWork.Add(new Player("Doug", "Bracewell", nzl));
                unitOfWork.Add(new Player("Kyle", "Mills", nzl));
                unitOfWork.Add(new Player("Chris", "Martin", nzl));
                unitOfWork.Add(new Player("Nathan", "McCullum", nzl));

                unitOfWork.Complete();
            }
        }
    }
}