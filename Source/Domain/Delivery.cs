using System;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.Domain
{
    // Delivery is Value type. not an entity
    // Immutable
    public class Delivery : ValueObject // or ValueObject<Delivery> so we dont need Equals implementation here
    {
        public virtual Player Bowler { get; private set; }

        public virtual Player Batter { get; private set; }

        public virtual int RunsScored { get; private set; } // naive; what about team vs player runs?

        internal Delivery(Player bowler, Player batter, int runsScored)
        {
            if (bowler == null) throw new ArgumentNullException("bowler");
            if (batter == null) throw new ArgumentNullException("batter");
            if (runsScored < 0) throw new ArgumentOutOfRangeException("runsScored", @"Cannot score less than 0 runs!");

            Bowler = bowler;
            Batter = batter;
            RunsScored = runsScored;
        }

        // for NH
        protected Delivery()
        {
        }

        public bool Equals(Delivery other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Bowler, Bowler) && Equals(other.Batter, Batter) && other.RunsScored == RunsScored;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Delivery)) return false;
            return Equals((Delivery) obj);
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

        // we overload the == and != operator for value types (but not for entities)
        public static bool operator ==(Delivery left, Delivery right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Delivery left, Delivery right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0} to {1}, {2} runs scored", Bowler, Batter, RunsScored);
        }
    }
}