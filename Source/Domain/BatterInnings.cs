using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class BatterInnings : Entity
    {
        public virtual TeamInnings TeamInnings { get; private set; }

        public virtual Player Batter { get; private set; }

        public virtual int BallsFaced { get; private set; }

        public virtual int RunsScored { get; private set; }

        public virtual bool NotOut { get; private set; }

        internal BatterInnings(TeamInnings teamInnings, Player batter)
        {
            if (teamInnings == null) throw new ArgumentNullException("teamInnings");
            if (batter == null) throw new ArgumentNullException("batter");

            TeamInnings = teamInnings;
            Batter = batter;
            NotOut = true;
        }

        public virtual void BallFaced(Ball ball)
        {
            if (ball == null) throw new ArgumentNullException("ball");

            BallsFaced++;
            RunsScored += ball.RunsScored;
        }

        public virtual void Dismiss() // method of dismissal?
        {
            NotOut = false;
        }

        // for NH rehydration
        protected BatterInnings()
        {
        }
    }
}