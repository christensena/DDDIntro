﻿namespace DDDIntro.Domain.Abstract
{
    public abstract class EntityWithGeneratedId
    {
        public virtual int Id { get; private set; }

        public virtual bool Equals(EntityWithGeneratedId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EntityWithGeneratedId)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}