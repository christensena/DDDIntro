using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Over : Entity
    {
        private IList<Ball> balls = new List<Ball>();

        public virtual Player Bowler { get; private set; }

        public virtual IEnumerable<Ball> Balls
        {
            get { return balls.ToArray(); }
        }

        internal Over(Player bowler)
        {
            Bowler = bowler;
        }

        public virtual bool IsOver()
        {
            return balls.Count() == 6; // naive; what about no balls, etc?
        }

        public virtual void RecordDelivery(Player batter, int runsScored)
        {
            if (batter == null) throw new ArgumentNullException("batter");
            if (IsOver()) throw new InvalidOperationException();
            // if had reference to innings here, could verify that batter was in the team batting!

            balls.Add(new Ball(Bowler, batter, runsScored));
        }

        // for NH rehydration only
        protected Over()
        {
        }
    }
}