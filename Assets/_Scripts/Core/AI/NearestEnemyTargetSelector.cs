using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Bresenham;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    [Serializable]
    public class NearestEnemyTargetSelector : ITargetSelector
    {
        [SerializeField] private int m_maxRange = 1;

        public AbilityTarget SelectTarget(IBoardGameWorld world, int executorId)
        {
            AIUnitModel target = TargetSelectorHelper.SelectNearestTarget(world, executorId);

            if (target == null)
                return AbilityTarget.None;

            Vector3 executorPosition = world.GetEntityPosition(executorId);
            Vector3 targetTilePosition = world.GetEntityPosition(target.Id);

            if (world.GetDistance(executorId, target.Id) <= m_maxRange && IsInLineOfSight(world, executorPosition.ToVector2Int(), targetTilePosition.ToVector2Int()))
                return AbilityTarget.EntityTarget(target.Id, new List<Vector3>());

            bool foundPath = PathFinder.TryFindPathToEntity(world, executorPosition, targetTilePosition, out List<Vector3> path);

            if (!foundPath)
                return AbilityTarget.None;

            int lastIndex = Mathf.Clamp(path.Count - m_maxRange + 1, 0, path.Count - 1);

            while (lastIndex < path.Count - 1 && !IsInLineOfSight(world, path[lastIndex].ToVector2Int(), targetTilePosition.ToVector2Int()))
                ++lastIndex;

            var trimmed = path.GetRange(0, lastIndex + 1);
            return AbilityTarget.EntityTarget(target.Id, trimmed);
        }

        private bool IsInLineOfSight(IBoardGameWorld world, Vector2Int from, Vector2Int to)
        {
            IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(from, to);
            foreach (Vector2Int tile in tiles)
            {
                if (tile == from || tile == to)
                    continue;

                BoardNodeType tileType = world.GetNodeType(tile.ToVector2());
                bool isObstacle = tileType == BoardNodeType.Obstacle;
                
                if (isObstacle)
                    return false;
            }
            return true;
        }
    }
}