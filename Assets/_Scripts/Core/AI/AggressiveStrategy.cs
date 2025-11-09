using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using System;

namespace MagmaHeart.Core.AI
{
    public class AggressiveStrategy : Strategy
    {
        public AggressiveStrategy(int lookAhead, Func<StateSnapshot, AIUnit> playerTargetSelection, AIUnit player) : base(lookAhead, playerTargetSelection, player) { }

        public override float EvaluateState(StateSnapshot state)
        {
            return 0;
        }
    }
}
