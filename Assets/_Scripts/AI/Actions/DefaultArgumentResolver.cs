using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    internal class DefaultArgumentResolver : IArgumentResolver
    {
        public IEnumerable<ActionArgs> Resolve(UnitAction action, AIUnitModel executor, SimulatedBoardState state)
        {
            IEnumerable<AIUnitModel> targets = state.Board.GetUnits()
                .Where(u => u != executor)
                .Where(u => executor.IsPlayer != u.IsPlayer);

            return action.CreateSimulationArguments(state, executor, targets);
        }
    }
}
