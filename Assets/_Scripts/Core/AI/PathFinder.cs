using System.Collections.Generic;
using MagmaHeart.AI;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    public static class PathFinder
    {
        public static bool TryFindPathToEntity(IBoardGameWorld world, Vector3 executorPosition, Vector3 targetTilePosition, out List<Vector3> path)
        {
            var result = false;
            path = null;

            IEnumerable<Vector3> candidates = new[]
            {
                targetTilePosition + Vector3.up,
                targetTilePosition + Vector3.down,
                targetTilePosition + Vector3.left,
                targetTilePosition + Vector3.right,
            };

            foreach (Vector3 candidate in candidates)
            {
                var tmpPath = new List<Vector3>();
                if (world.TryFindPath(executorPosition, candidate, out tmpPath))
                {
                    if (path == null)
                        path = tmpPath;
                    
                    if (tmpPath.Count < path.Count)
                        path = tmpPath;

                    result = true;
                }
            }

            return result;
        }
    }
}