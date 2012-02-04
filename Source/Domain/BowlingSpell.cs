using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class BowlingSpell : Entity
    {
        private IList<Over> overs = new List<Over>();

        public virtual Player Bowler { get; private set; }

        public virtual IEnumerable<Over> Overs
        {
            get { return overs.ToArray(); }
        }

        internal BowlingSpell(Player bowler)
        {
            Bowler = bowler;
        }

        // wanted to make this internal
        public virtual void RecordOverCommenced(Over over)
        {
            if (over == null) throw new ArgumentNullException("over");
            if (! over.Bowler.Equals(Bowler)) throw new ArgumentException("Over has different bowler!");

            overs.Add(over);
        }

        public virtual int GetRunsConceded()
        {
            return overs.Sum(o => o.RunsScored());
        }

        // for NH rehydration
        protected BowlingSpell()
        {
        }
    }
}