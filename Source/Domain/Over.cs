using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Over : Entity
    {
        private IList<Ball> balls = new List<Ball>();

        public virtual TeamInnings BattingTeamInnings { get; private set; }

        public virtual Player Bowler { get; private set; }

        public virtual IEnumerable<Ball> Balls
        {
            get { return balls.ToArray(); }
        }

        internal Over(TeamInnings battingTeamInnings, Player bowler)
        {
            BattingTeamInnings = battingTeamInnings;
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

            var ball = new Ball(Bowler, batter, runsScored);

            var batterInnings = BattingTeamInnings.GetBatterInnings(batter);
            if (! batterInnings.NotOut)
                throw new InvalidOperationException("Batter is out!");

            balls.Add(ball);
            batterInnings.BallFaced(ball);
        }

        // for NH rehydration only
        protected Over()
        {
        }
    }
}