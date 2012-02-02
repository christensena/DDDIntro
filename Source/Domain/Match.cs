using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    // Match is an aggregate root
    public class Match : Entity
    {
        private IList<TeamInnings> innings = new List<TeamInnings>();

        public virtual DateTime Date { get; private set; }

        public virtual Team Team1 { get; private set; }
        
        public virtual Team Team2 { get; private set; }

        public virtual IEnumerable<TeamInnings> Innings
        {
            get { return innings.ToArray(); }
        }

        public Match(DateTime date, Team team1, Team team2)
        {
            if (team1 == null) throw new ArgumentNullException("team1");
            if (team2 == null) throw new ArgumentNullException("team2");
            Date = date;
            Team1 = team1;
            Team2 = team2;
        }

        // for NH rehydration only
        protected Match()
        {
        }

        public virtual TeamInnings NewInnings(Team battingTeam)
        {
            if (battingTeam == null) throw new ArgumentNullException("battingTeam");
            if (! (battingTeam.Equals(Team1) || battingTeam.Equals(Team2))) 
                throw new InvalidOperationException("Team must be one of the teams in the match!");

            TeamInnings teamInnings;

            if (battingTeam.Equals(Team1))
            {
                teamInnings = new TeamInnings(Team1, Team2);
            }
            else
            {
                teamInnings = new TeamInnings(Team2, Team1);
            }

            innings.Add(teamInnings);

            return teamInnings;
        }
    }

    public class PlayerInnings : Entity
    {
        
    }
}