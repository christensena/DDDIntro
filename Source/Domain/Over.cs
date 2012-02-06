﻿using System;
using System.Collections.Generic;
using System.Linq;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    public class Over : Entity
    {
        private IList<Delivery> deliveries = new List<Delivery>();

        public virtual TeamInnings BattingTeamInnings { get; private set; }

        public virtual Player Bowler { get; private set; }

        public virtual IEnumerable<Delivery> Deliveries
        {
            get { return deliveries.ToArray(); }
        }

        internal Over(TeamInnings battingTeamInnings, Player bowler)
        {
            BattingTeamInnings = battingTeamInnings;
            Bowler = bowler;
        }

        public virtual bool IsOver()
        {
            return deliveries.Count() == 6; // naive; what about no-balls, etc?
        }

        public virtual int RunsScored()
        {
            return deliveries.Sum(b => b.RunsScored);
        }

        public virtual bool IsMaiden()
        {
            return RunsScored() == 0;
        }

        public virtual void RecordDelivery(Player batter, int runsScored)
        {
            if (batter == null) throw new ArgumentNullException("batter");
            if (IsOver()) throw new InvalidOperationException("Over is over so can't fit more deliveries!");

            var batterInnings = BattingTeamInnings.GetBatterInnings(batter);
            if (! batterInnings.NotOut)
                throw new InvalidOperationException("Batter is out!");

            var delivery = new Delivery(Bowler, batter, runsScored);
            deliveries.Add(delivery);
            batterInnings.BallFaced(delivery);
        }

        // for NH rehydration only
        protected Over()
        {
        }
    }
}