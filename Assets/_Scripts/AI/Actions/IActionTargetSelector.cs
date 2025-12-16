using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    public interface IActionTargetSelector
    {
        public IEnumerable<AIUnitModel> SelectTargets(BoardState state, AIUnitModel executor);
    }
}
