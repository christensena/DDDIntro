using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class BatterInnings : EntityWithGeneratedId
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

        public virtual void BallFaced(Delivery delivery)
        {
            if (delivery == null) throw new ArgumentNullException("delivery");
            if (! NotOut) throw new InvalidOperationException("Cannot face a Delivery after being dismissed!");

            BallsFaced++;
            RunsScored += delivery.RunsScored;
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