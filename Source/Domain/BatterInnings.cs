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

        public virtual DateTime StartTime { get; private set; }

        public virtual DateTime? EndTime { get; private set; }

        internal BatterInnings(TeamInnings teamInnings, Player batter)
        {
            if (teamInnings == null) throw new ArgumentNullException("teamInnings");
            if (batter == null) throw new ArgumentNullException("batter");

            TeamInnings = teamInnings;
            Batter = batter;
            NotOut = true;
            StartTime = DateTime.Now;
        }

        public virtual void BallFaced(Ball ball)
        {
            if (ball == null) throw new ArgumentNullException("ball");
            if (! NotOut) throw new InvalidOperationException("Cannot face a ball after being dismissed!");

            BallsFaced++;
            RunsScored += ball.RunsScored;
        }

        public virtual TimeSpan GetDuration()
        {
            return (EndTime ?? DateTime.Now).Subtract(StartTime);
        }

        // this is an area not started yet; will change drastically from this
        public virtual void Dismiss() // method of dismissal?
        {
            NotOut = false;
            EndTime = DateTime.Now;
        }

        // for NH rehydration
        protected BatterInnings()
        {
        }
    }
}