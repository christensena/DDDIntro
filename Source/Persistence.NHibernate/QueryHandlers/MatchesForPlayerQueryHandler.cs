using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain;
using DDDIntro.Domain.Services;
using DDDIntro.Domain.Services.Queries;
using NHibernate;

namespace DDDIntro.Persistence.NHibernate.QueryHandlers
{
    // example of a query handler that needs advanced, persistence mechanism specific
    // facilities for performance or other reasons
    //
    // it is preferable to have query handlers in the Domain.Services as they
    // are then easier to test and not as dependent on particular ORMs or data stores
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

            // we could use NHibernate QueryOver, HQL or plain SQL or stored procedures, etc
            //
            // or there is the example of using a full text index instead of Contains() which spits out an 
            // inefficient "LIKE '%'+value+'%'" clause.
            // that example may be database engine specific rather than just ORM specific so it might break our integration tests.
            //
            // in this example we are using plain SQL but this will need maintenance if our model changes
            // which is the problem with micro-ORMs as well!
            var sqlQuery = session.CreateSQLQuery(
                  @"SELECT Match.* FROM Match 
                    INNER JOIN Team ON Team.Match_Id=Match.Id 
                    INNER JOIN PlayerToTeam ON Team_Id=Team.Id
                    WHERE PlayerToTeam.Player_Id = ?");
            sqlQuery.SetInt32(0, query.PlayerID);

            var list = sqlQuery.AddEntity(typeof (Match)).List();

            return ConvertToTypedEnumerable<Match>(list).ToArray();
        }

        private static IEnumerable<T> ConvertToTypedEnumerable<T>(IList list) where T : class
        {
            return (from object item in list select item).Cast<T>();
        }
    }
}