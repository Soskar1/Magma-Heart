using System;

namespace MagmaHeart.AI.Reasoning
{
    public interface IAIUnit : IEquatable<IAIUnit>
    {
        public bool IsPlayer { get; }
        public IAction[] PossibleActions { get; }
    }
}
