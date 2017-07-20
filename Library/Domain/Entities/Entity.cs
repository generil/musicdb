using System;

namespace Music.Domain.Entities
{
    public abstract class Entity
    {
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Entity e = (Entity)obj;
            return this.Id == e.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}