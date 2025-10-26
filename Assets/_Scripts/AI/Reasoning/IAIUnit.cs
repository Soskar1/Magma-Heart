using System;

namespace MagmaHeart.AI.Reasoning
{
    public interface IAIUnit : IEquatable<IAIUnit>
    {
        public IAction[] PossibleActions { get; }
    }
}
