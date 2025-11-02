using System;

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

        public virtual PropertyList GetPropertySnapshots()
        {
            PropertyList list = new PropertyList();
            IsAliveProperty isAliveProperty = new IsAliveProperty(true);

            list.Add(isAliveProperty);

            return list;
        }
    }
}
