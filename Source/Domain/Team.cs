using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Team : EntityWithGeneratedId
    {
        private IList<Player> members = new List<Player>();
        private Match match;
        private Country country;
        private Player twelfthMan;

        public virtual Match Match
        {
            get { return match; }
        }

        public virtual Country Country
        {
            get { return country; }
        }

        public virtual Player TwelfthMan
        {
            get { return twelfthMan; }
        }

        public virtual IEnumerable<Player> Members
        {
            get { return members.ToArray(); }
        }

        internal Team(Match match, Country country)
        {
            this.match = match;
            this.country = country;
        }

        public virtual void AddMember(Player player)
        {
            if (IsPlayerAlreadyInTeam(player)) throw new ArgumentException("Player is already in the team!");
            if (IsTeamComplete()) throw new InvalidOperationException("Maximum of 11 players in a team, plus 12th man");

            if (members.Count < 11)
            {
                members.Add(player);
            }
            else
            {
                twelfthMan = player;
            }
        }

        public virtual void RemoveMember(Player player)
        {
            if (! IsPlayerAlreadyInTeam(player)) throw new InvalidOperationException("Player not in team!");

            if (player.Equals(TwelfthMan))
            {
                twelfthMan = null;
            }
            else
            {
                members.Remove(player);
            }
        }

        public virtual bool IsTeamComplete()
        {
            return members.Count() == 11 && TwelfthMan != null;
        }

        public virtual bool IsPlayerAlreadyInTeam(Player player)
        {
            // notice I can use Contains() and Equals() instead of digging down to ID's.
            // this also works for value objects. Implement Equals()!
            return members.Contains(player) || player.Equals(TwelfthMan);
        }

        // for NH rehydration only
        protected Team()
        {
        }
    }
}