using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;

namespace DDDIntro.IntegrationTests.Persistence
{
    public class ContextSetUpHelper
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ContextSetUpHelper(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Player SetUpPlayer(string firstName, string lastName)
        {
            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var player = uow.GetAll<Player>().SingleOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
                if (player != null)
                    return player;

                player = new Player(firstName, lastName);

                uow.Add(player);

                uow.Complete();

                return player;
            }
        }

        public Country SetUpCountry(string countryName = "New Zealand")
        {
            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var country = uow.GetAll<Country>().SingleOrDefault(x => x.Name == countryName);
                if (country != null)
                    return country;

                country = new Country(countryName);

                uow.Add(country);

                uow.Complete();

                return country;
            }
        }
     
        public Team SetUpTeam(string countryName)
        {
            var country = SetUpCountry(countryName);

            var twelfthMan = SetUpPlayer("Jesse", "Ryder");

            var player1 = SetUpPlayer("Brendan", "McCullum");
            var player2 = SetUpPlayer("Chris", "Martin");

            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var team = new Team(country);

                team.AddMember(player1);
                team.AddMember(player2);

                team.TwelfthMan = twelfthMan;

                uow.Add(team);

                uow.Complete();

                return team;
            }
        }
    }
}