using System;

namespace DDDIntro.Domain.Services.Queries
{
    public class PlayersForCountryQuery : IQuery<Player[]>
    {
        public string CountryName { get; private set; }

        public PlayersForCountryQuery(string countryName)
        {
            if (countryName == null) throw new ArgumentNullException("countryName");
            CountryName = countryName;
        }
    }

    public class MatchesForPlayerQuery : IQuery<Match[]>
    {
        public int PlayerID { get; private set; }

        public MatchesForPlayerQuery(int playerID)
        {
            PlayerID = playerID;
        }
    }
}