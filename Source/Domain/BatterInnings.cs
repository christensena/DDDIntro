using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class BatterInnings : EntityWithGeneratedId
    {
        private TeamInnings teamInnings;
        private Player batter;
        private int ballsFaced;
        private int runsScored;
        private bool notOut;
        private DateTime startTime;
        private DateTime? endTime;

        public virtual TeamInnings TeamInnings
        {
            get { return teamInnings; }
        }

        public virtual Player Batter
        {
            get { return batter; }
        }

        public virtual int BallsFaced
        {
            get { return ballsFaced; }
        }

        public virtual int RunsScored
        {
            get { return runsScored; }
        }

        public virtual bool NotOut
        {
            get { return notOut; }
        }

        public virtual DateTime StartTime
        {
            get { return startTime; }
        }

        public virtual DateTime? EndTime
        {
            get { return endTime; }
        }

        internal BatterInnings(TeamInnings teamInnings, Player batter)
        {
            if (teamInnings == null) throw new ArgumentNullException("teamInnings");
            if (batter == null) throw new ArgumentNullException("batter");
            this.teamInnings = teamInnings;
            this.batter = batter;
            notOut = true;
            startTime = DateTime.Now;
        }

        public virtual void BallFaced(Delivery delivery)
        {
            if (delivery == null) throw new ArgumentNullException("delivery");
            if (! NotOut) throw new InvalidOperationException("Cannot face a Delivery after being dismissed!");

            ballsFaced++;
            runsScored += delivery.RunsScored;
        }

        public virtual TimeSpan GetDuration()
        {
            return (EndTime ?? DateTime.Now).Subtract(StartTime);
        }

        // this is an area not started yet; will change drastically from this
        public virtual void Dismiss() // method of dismissal?
        {
            notOut = false;
            endTime = DateTime.Now;
        }

        // for NH rehydration
        protected BatterInnings()
        {
        }
    }
}