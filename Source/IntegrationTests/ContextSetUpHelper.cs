using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;

namespace DDDIntro.IntegrationTests
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
            for (var i = 0; i < 22; i++)
            {
                SetUpRandomPlayer(countryName);
            }
        }
     
        public void PickTeam(Team team, IUniversalRepository universalRepository)
        {
            var countryName = team.Country.Name;
            var playersForCountry = universalRepository.GetAll<Player>().Where(p => p.Country.Name.Equals(countryName));

            var random = new Random();
            var availablePlayers = playersForCountry.ToList();
            while (availablePlayers.Any() && ! team.IsTeamComplete())
            {
                var pickedPlayer = availablePlayers[random.Next(availablePlayers.Count)];
                team.AddMember(pickedPlayer);
                availablePlayers.Remove(pickedPlayer);
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

        public Player SetUpRandomPlayer(string countryName)
        {
            return SetUpPlayer(randomGenerator.GetRandomString(), randomGenerator.GetRandomString(), countryName);
        }

        public int SetUpMatch()
        {
            return SetUpMatch(SetUpCountry("New Zealand"), SetUpCountry("South Africa"));
        }

        public int SetUpMatch(Country country1, Country country2)
        {
            PopulateCountryPlayerPool(country1.Name);
            PopulateCountryPlayerPool(country2.Name);

            using (var uow = unitOfWorkFactory.BeginUnitOfWork())
            {
                var match = Match.Create(DateTime.Today, country1, country2);

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