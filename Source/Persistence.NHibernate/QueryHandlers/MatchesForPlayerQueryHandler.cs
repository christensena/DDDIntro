using System;
using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain.Services.Queries;
using NHibernate;

namespace DDDIntro.Domain.Services.QueryHandlers
{
    public class MatchesForPlayerQueryHandler : IQueryHandler<MatchesForPlayerQuery, Match[]>
    {
        private readonly ISession session;

        public MatchesForPlayerQueryHandler(ISession session)
        {
            this.session = session;
        }

        public Match[] Handle(MatchesForPlayerQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");

            return session.QueryOver<Match>()
                .Where(m => m.Teams.Any(t => t.Members.Any(p => p.Id == query.PlayerID))).List().ToArray();
        }
    }
}