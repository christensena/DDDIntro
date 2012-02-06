using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;

namespace DDDIntro.IntegrationTests.Persistence
{
    public class ContextSetUpHelper
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly RandomsGenerator randomGenerator = new RandomsGenerator();

        public ContextSetUpHelper(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Country SetUpCountry(string countryName)
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

        public void PopulateCountryPlayerPool(string countryName)
        {
            SetUpPlayer("Jesse", "Ryder" + randomGenerator.GetRandomString(), countryName);
            SetUpPlayer("Brendan", "McCullum" + randomGenerator.GetRandomString(), countryName);
            SetUpPlayer("Chris", "Martin" + randomGenerator.GetRandomString(), countryName);
            SetUpPlayer("Martin", "Guptill" + randomGenerator.GetRandomString(), countryName);
            SetUpPlayer("Ross", "Taylor" + randomGenerator.GetRandomString(), countryName);
            SetUpPlayer("Daniel", "Vettori" + randomGenerator.GetRandomString(), countryName);
        }
     
        public void PickTeam(Team team, IAggregateRepository aggregateRepository)
        {
            var countryName = team.Country.Name;
            var playersForCountry = aggregateRepository.GetAll<Player>().Where(p => p.Country.Name.Equals(countryName));

            var random = new Random();
            var availablePlayers = playersForCountry.ToList();
            while (availablePlayers.Any() && ! team.IsTeamComplete())
            {
                var pickedPlayer = availablePlayers[random.Next(availablePlayers.Count)];
                team.AddMember(pickedPlayer);
                availablePlayers.Remove(pickedPlayer);
            }

            if (availablePlayers.Any())
            {
                team.TwelfthMan = availablePlayers[random.Next(availablePlayers.Count)];
            }
        }

        public Player SetUpPlayer(string firstName, string lastName, string countryName)
        {
            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var country = uow.GetAll<Country>().Single(x => x.Name == countryName);

                var player = new Player(firstName, lastName, country);

                uow.Add(player);

                uow.Complete();

                return player;
            }
        }

        public int SetUpMatch(Country country1, Country country2)
        {
            PopulateCountryPlayerPool(country1.Name);
            PopulateCountryPlayerPool(country2.Name);

            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var match = new Match(DateTime.Today, country1, country2);

                PickTeam(match.Team1, uow);
                PickTeam(match.Team2, uow);

                uow.Add(match);

                uow.Complete();

                return match.Id;
            }
        }
        private class RandomsGenerator
        {
            private readonly Random random = new Random();

            public string GetRandomString()
            {
                return random.Next(int.MaxValue).ToString();
            }
        }
    }
}