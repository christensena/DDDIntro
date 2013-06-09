using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class TeamInnings : EntityWithGeneratedId
    {
        private IList<Over> overs = new List<Over>();
        private IList<BatterInnings> batterInnings = new List<BatterInnings>();
        private IList<BowlingSpell> bowlingSpells = new List<BowlingSpell>();
        private Team battingTeam;
        private Team fieldingTeam;

        public virtual Team BattingTeam
        {
            get { return battingTeam; }
        }

        public virtual Team FieldingTeam
        {
            get { return fieldingTeam; }
        }

        public virtual IEnumerable<Over> Overs
        {
            get { return overs.ToArray(); }
        }

        public virtual IEnumerable<BatterInnings> BatterInnings
        {
            get { return batterInnings.ToArray(); }
        }

        public virtual IEnumerable<BowlingSpell> BowlingSpells
        {
            get { return bowlingSpells.ToArray(); }
        }

        //public virtual Player FacingBatter { get; private set; }

        internal TeamInnings(Team battingTeam, Team fieldingTeam)
        {
            this.battingTeam = battingTeam;
            this.fieldingTeam = fieldingTeam;
        }

        public virtual BatterInnings CommenceBatterInnings(Player batter)
        {
            if (batter == null) throw new ArgumentNullException("batter");
            if (! BattingTeam.Members.Contains(batter))
                throw new InvalidOperationException("Player not on batting team!");
            if (batterInnings.Any(b => b.Batter.Equals(batter)))
                throw new InvalidOperationException("Player already commenced batting!");

            var batterInningsSingular = new BatterInnings(this, batter);
            batterInnings.Add(batterInningsSingular);
            return batterInningsSingular;
        }

        public virtual BatterInnings GetBatterInnings(Player batter)
        {
            if (batter == null) throw new ArgumentNullException("batter");
            var foundBatterInnings = batterInnings.SingleOrDefault(b => b.Batter.Equals(batter));
            if (foundBatterInnings == null)
                throw new InvalidOperationException("Batting not commenced for player: " + batter);

            return foundBatterInnings;
        }

        public virtual Over NewOver(Player bowler)
        {
            if (bowler == null) throw new ArgumentNullException("bowler");
            if (! FieldingTeam.Members.Contains(bowler))
                throw new InvalidOperationException("Player not a member of the fielding team! " + bowler);

            var over = new Over(this, bowler); 
            overs.Add(over);

            GetBowlingSpell(bowler).RecordOverCommenced(over);

            return over;
        }

        //public virtual Player GetFacingBatsman()
        //{
        //    if (! batterInnings.Any()) throw new InvalidOperationException("No innings commenced!");
        //    if (batterInnings.Count(x => x.NotOut) <= 1) throw new InvalidOperationException("10 wickets down. No batsman to face");


        //}

        public virtual Over GetCurrentOver()
        {
            if (! overs.Any()) throw new InvalidOperationException("No overs!");
            var lastOver = overs.Last();
            if (lastOver.IsOver()) throw new InvalidOperationException("Last over completed. Need to start a new over.");

            return lastOver;
        }

        public virtual int GetScore()
        {
            return overs.Sum(o => o.RunsScored());
        }

        private BowlingSpell GetBowlingSpell(Player bowler)
        {
            var bowlingSpell = bowlingSpells.SingleOrDefault(bs => bs.Bowler.Equals(bowler));
            if (bowlingSpell == null)
            {
                bowlingSpell = new BowlingSpell(bowler);
                bowlingSpells.Add(bowlingSpell);
            }
            return bowlingSpell;
        }

        // for NH rehydration
        protected TeamInnings()
        {
        }
    }
}