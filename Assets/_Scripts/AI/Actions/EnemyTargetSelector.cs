using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    public class EnemyTargetSelector : IActionTargetSelector
    {
        public IEnumerable<AIUnitModel> SelectTargets(BoardState state, AIUnitModel executor)
        {
            return state.Board.GetUnits()
                .Where(u => u != executor)
                .Where(u => executor.IsPlayer != u.IsPlayer);
        }
    }
}
