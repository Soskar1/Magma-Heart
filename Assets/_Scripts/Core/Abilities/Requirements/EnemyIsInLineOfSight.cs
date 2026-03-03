using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.Bresenham;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Requirements
{
    [System.Serializable]
    public class EnemyIsInLineOfSight : IAbilityRequirement<IBoardGameWorld>
    {
        public bool IsMet(IBoardGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);

            if (!isTargetingEntity)
                return false;

            Vector2Int executorPosition = world.GetEntityPosition(executorId).ToVector2Int();
            Vector2Int targetPosition = world.GetEntityPosition(target.EntityId).ToVector2Int();

            IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(executorPosition, targetPosition);

            foreach (Vector2Int tile in tiles)
            {
                BoardNodeType tileType = world.GetNodeType(tile.ToVector2());
                bool isObstacle = tileType == BoardNodeType.Obstacle;

                if (isObstacle && tile != executorPosition && tile != targetPosition)
                    return false;
            }

            return true;
        }
    }
}
