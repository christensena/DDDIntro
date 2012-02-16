using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;

namespace DDDIntro.Application.Services
{
    public class TestDataGenerator
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public TestDataGenerator(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
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
                unitOfWork.Add(new Country("New Zealand"));
                unitOfWork.Add(new Country("Australia"));
                unitOfWork.Add(new Country("South Africa"));
                unitOfWork.Add(new Country("India"));
                unitOfWork.Add(new Country("England"));
                unitOfWork.Add(new Country("West Indies"));
                unitOfWork.Add(new Country("Sri Lanka"));
                unitOfWork.Add(new Country("Pakistan"));
                unitOfWork.Add(new Country("Bangladesh"));
                unitOfWork.Add(new Country("Zimbabwe"));

                unitOfWork.Complete();
            }
        }

        public void GenerateTeams()
        {
            using (var unitOfWork = unitOfWorkFactory.BeginUnitOfWork())
            {
                var nzl = unitOfWork.GetAll<Country>().SingleOrDefault(c => c.Name == "New Zealand");

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