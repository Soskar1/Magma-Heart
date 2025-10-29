using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public class AIUnit : IEquatable<AIUnit>
    {
        public bool IsPlayer { get; init; }
        public Action[] PossibleActions { get; init; }

        public bool Equals(AIUnit other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return false;
        }

        public virtual Dictionary<Type, PropertySnapshot> GetPropertySnapshots()
        {
            IsAliveProperty isAliveProperty = new IsAliveProperty(true);
            return new Dictionary<Type, PropertySnapshot> { { typeof(IsAliveProperty), isAliveProperty } };
        }
    }
}
