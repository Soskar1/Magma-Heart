using System.Collections.Generic;
using System.Linq;
using MagmaHeart.AI;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    public static class PathFinder
    {
        public static bool TryFindPathToEntity(IBoardGameWorld world, Vector3 executorPosition, Vector3 targetTilePosition, out List<Vector3> path)
        {
            IEnumerable<Vector3> sortedCandidates = new[]
            {
                targetTilePosition + Vector3.up,
                targetTilePosition + Vector3.down,
                targetTilePosition + Vector3.left,
                targetTilePosition + Vector3.right,
            }.OrderBy(candidate => Vector3.SqrMagnitude(executorPosition - (Vector3)candidate));

            foreach (Vector3 candidate in sortedCandidates)
            {
                if (world.TryFindPath(executorPosition, candidate, out path))
                    return true;
            }

            path = null;
            return false;
        }
    }
}