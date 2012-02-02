using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class TeamInnings : Entity
    {
        private IList<Over> overs = new List<Over>();

        public virtual Team BattingTeam { get; private set; }

        public virtual Team FieldingTeam { get; private set; }

        public virtual IEnumerable<Over> Overs
        {
            get { return overs.ToArray(); }
        }

        internal TeamInnings(Team battingTeam, Team fieldingTeam)
        {
            BattingTeam = battingTeam;
            FieldingTeam = fieldingTeam;
        }

        public virtual Over NewOver(Player bowler)
        {
            if (bowler == null) throw new ArgumentNullException("bowler");
            var over = new Over(bowler);
            overs.Add(over);
            return over;
        }

        // for NH rehydration
        protected TeamInnings()
        {
        }
    }
}