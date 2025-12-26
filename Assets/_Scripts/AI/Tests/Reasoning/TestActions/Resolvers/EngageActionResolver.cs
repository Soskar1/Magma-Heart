using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageActionResolver : IActionResolver
    {
        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            EngageActionData data = (EngageActionData)definition.Data;

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                Position attackerPos = state.GetProperty<Position>(executor);
                Position targetPos = state.GetProperty<Position>(potentialTarget);

                float distance = attackerPos.Distance(targetPos);
                if (distance > 1 && distance <= data.Speed + 1)
                {
                    resolvedArgs = new EngageActionArgs(executor, potentialTarget, data);
                    return true;
                }
            }

            resolvedArgs = null;
            return false;
        }
    }
}