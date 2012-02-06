using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Team : Entity
    {
        private IList<Player> members = new List<Player>();

        public virtual Match Match { get; private set; }

        public virtual Country Country { get; private set; }

        public virtual Player TwelfthMan { get; set; }

        public virtual IEnumerable<Player> Members
        {
            get { return members.ToArray(); }
        }

        internal Team(Match match, Country country)
        {
            Match = match;
            Country = country;
        }

        // for NH rehydration only
        protected Team()
        {
        }


        public virtual void AddMember(Player player)
        {
            if (IsTeamComplete()) throw new InvalidOperationException("Maximum of 11 players in a team");
            if (IsPlayerAlreadyInTeam(player)) throw new InvalidOperationException("Player is already in the team!");
            members.Add(player);
        }

        public virtual void RemoveMember(Player player)
        {
            members.Remove(player);
        }

        public virtual bool IsTeamComplete()
        {
            return members.Count() == 11;
        }

        public virtual bool IsPlayerAlreadyInTeam(Player player)
        {
            return TwelfthMan == player || members.Contains(player);
        }
    }
}