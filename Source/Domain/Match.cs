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
        private IList<Team> teams = new List<Team>();

        public virtual DateTime Date { get; private set; }

        public virtual Team Team1
        {
            get { return teams[0]; }
        }

        public virtual Team Team2
        {
            get { return teams[1]; }
        }

        public virtual IEnumerable<Team> Teams { get { return teams.ToArray(); } }

        public virtual IEnumerable<TeamInnings> Innings
        {
            get { return innings.ToArray(); }
        }

        public Match(DateTime date, Country team1Country, Country team2Country)
        {
            if (team1Country == null) throw new ArgumentNullException("team1Country");
            if (team2Country == null) throw new ArgumentNullException("team2Country");
            if (team1Country.Equals(team2Country)) throw new ArgumentException("Teams must not be the same country!");
            Date = date;
            teams.Add(new Team(this, team1Country));
            teams.Add(new Team(this, team2Country));
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
}