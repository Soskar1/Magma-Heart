using System.Collections.Generic;
using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Bresenham;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using UnityEngine;
using MagmaHeart.Extensions;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class RangedAttackActionResolver : IActionResolver
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

                Vector2Int attackerPosition = state.GetProperty<PositionPropertySnapshot>(executor).Position.ToVector2Int();
                Vector2Int targetPosition = state.GetProperty<PositionPropertySnapshot>(potentialTarget).Position.ToVector2Int();
                IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(attackerPosition, targetPosition);

                bool wallBetweenEntities = false;
                foreach (Vector2Int tile in tiles)
                {
                    bool isObstacle = state.Board.GetNodeType(tile) == BoardNodeType.Obstacle;

                    if (isObstacle && tile != attackerPosition && tile != targetPosition)
                    {
                        wallBetweenEntities = true;
                        break;
                    }
                }

                if (!wallBetweenEntities)
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