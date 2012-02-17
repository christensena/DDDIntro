using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain.Services.Queries;

namespace DDDIntro.Domain.Services.QueryHandlers
{
    public class PlayersForCountryQueryHandler : IQueryHandler<PlayersForCountryQuery, Player[]>
    {
        private readonly IRepository<Player> repository;

        // some might use session directly here
        public PlayersForCountryQueryHandler(IRepository<Player> repository)
        {
            this.repository = repository;
        }

        public Player[] Handle(PlayersForCountryQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");

            return repository.FindAll()
                .Where(p => p.Country.Name == query.CountryName)
                .ToArray();
        }
    }
}