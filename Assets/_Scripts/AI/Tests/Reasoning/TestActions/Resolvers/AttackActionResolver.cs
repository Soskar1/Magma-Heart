using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackActionResolver : IActionResolver
    {
        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            AttackActionData data = (AttackActionData)definition.Data;

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                Position attackerPos = state.GetProperty<Position>(executor);
                Position targetPos = state.GetProperty<Position>(potentialTarget);

                if (attackerPos.Distance(targetPos) <= 1)
                {
                    resolvedArgs = new AttackActionArgs(executor, potentialTarget, data);
                    return true;
                }
            }

            resolvedArgs = null;
            return false;
        }
    }
}