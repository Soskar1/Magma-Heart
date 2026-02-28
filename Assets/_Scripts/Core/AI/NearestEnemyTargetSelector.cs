using System;
using System.Collections.Generic;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    [Serializable]
    public class NearestEnemyTargetSelector : ITargetSelector
    {
        public AbilityTarget SelectTarget(IBoardGameWorld world, int executorId)
        {
            float minDistance = float.MaxValue;
            AIUnitModel target = null;

            foreach (AIUnitModel unit in world.GetUnits())
            {
                if (unit.Id == executorId)
                    continue;

                if (!world.AreEnemiesToEachOther(unit.Id, executorId))
                    continue;

                float distance = world.GetDistance(executorId, unit.Id);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = unit;
                }
            }
            if (target == null)
                return AbilityTarget.None();

            Vector3 executorPosition = world.GetEntityPosition(executorId);
            Vector3 targetTilePosition = world.GetEntityPosition(target.Id);

            bool foundPath = PathFinder.TryFindPathToEntity(world, executorPosition, targetTilePosition, out List<Vector3> path);

            if (!foundPath)
                return AbilityTarget.None();

            return AbilityTarget.EntityTarget(target.Id, path);
        }
    }
}