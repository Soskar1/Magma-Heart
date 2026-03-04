using System;
using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    [Serializable]
    public class PossiblePathToNearestEnemyTargetSelector : ITargetSelector
    {
        [SerializeField] private ParameterDatabase m_database;
        [SerializeField] private int m_tilesPerResource;

        public AbilityTarget SelectTarget(IBoardGameWorld world, int executorId)
        {
            IParameter energy = world.GetParameter(executorId, m_database.Energy);
            if (energy.CurrentValue <= 0)
                return AbilityTarget.None;

            AIUnitModel target = TargetSelectorHelper.SelectNearestTarget(world, executorId);

            if (target == null)
                return AbilityTarget.None;

            Vector3 executorPosition = world.GetEntityPosition(executorId);
            Vector3 targetTilePosition = world.GetEntityPosition(target.Id);

            bool foundPath = PathFinder.TryFindPathToEntity(world, executorPosition, targetTilePosition, out List<Vector3> path);

            if (!foundPath)
                return AbilityTarget.None;

            IParameter speed = world.GetParameter(executorId, m_database.Speed);
            int totalTilesPerResource = m_tilesPerResource + (int)speed.CurrentValue;
            int energyCost = Mathf.CeilToInt(path.Count / (float)totalTilesPerResource);
            int maxEnergyToSpend = Mathf.Min(energyCost, (int)energy.CurrentValue);

            if (maxEnergyToSpend <= 0)
                return AbilityTarget.None;

            return AbilityTarget.PathTarget(path.Take(maxEnergyToSpend * totalTilesPerResource).ToList());
        }
    }
}
