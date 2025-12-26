using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackActionResolver : IActionResolver
    {
        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            AttackActionData data = (AttackActionData)definition.Data;

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                if (executor.IsPlayer && potentialTarget.IsPlayer)
                    continue;

                if (!executor.IsPlayer && !potentialTarget.IsPlayer)
                    continue;

                PositionPropertySnapshot attackerPos = state.GetProperty<PositionPropertySnapshot>(executor);
                PositionPropertySnapshot targetPos = state.GetProperty<PositionPropertySnapshot>(potentialTarget);

                if (attackerPos.ManhattanDistance(targetPos) <= data.AttackDistance)
                {
                    resolvedArgs = new AttackActionArgs((EntityModel)executor, (EntityModel)potentialTarget, data);
                    return true;
                }
            }

            resolvedArgs = null;
            return false;
        }
    }
}