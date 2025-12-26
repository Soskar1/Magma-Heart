using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayActionResolver : IActionResolver
    {
        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            RunAwayActionData data = (RunAwayActionData)definition.Data;

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                resolvedArgs = new RunAwayActionArgs(executor, potentialTarget, data);
                return true;
            }

            resolvedArgs = null;
            return false;
        }
    }
}
