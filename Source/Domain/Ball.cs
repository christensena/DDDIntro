using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    // Ball is Value type. not an entity
    // Immutable
    public class Ball : ValueObject
    {
        public virtual Player Bowler { get; private set; }

        public virtual Player Batter { get; private set; }

        public virtual int RunsScored { get; private set; } // naive; what about team vs player runs?

        internal Ball(Player bowler, Player batter, int runsScored)
        {
            if (bowler == null) throw new ArgumentNullException("bowler");
            if (batter == null) throw new ArgumentNullException("batter");
            if (runsScored < 0) throw new ArgumentOutOfRangeException("runsScored", @"Cannot score less than 0 runs!");

            Bowler = bowler;
            Batter = batter;
            RunsScored = runsScored;
        }

        // for NH
        protected Ball()
        {
        }

        public bool Equals(Ball other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Bowler, Bowler) && Equals(other.Batter, Batter) && other.RunsScored == RunsScored;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Ball)) return false;
            return Equals((Ball) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Bowler.GetHashCode();
                result = (result*397) ^ Batter.GetHashCode();
                result = (result*397) ^ RunsScored;
                return result;
            }
        }

        public static bool operator ==(Ball left, Ball right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Ball left, Ball right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0} to {1}, {2} runs scored", Bowler, Batter, RunsScored);
        }
    }
}