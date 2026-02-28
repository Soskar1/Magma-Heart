using System;
using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
            int maxEnergyToSpend = Mathf.CeilToInt(path.Count / (float)totalTilesPerResource);

            IParameter energy = world.GetParameter(executorId, m_database.Energy);
            int availableEnergy = Mathf.Min(maxEnergyToSpend, (int)energy.CurrentValue);

            return AbilityTarget.PathTarget(path.Take(availableEnergy * totalTilesPerResource).ToList());
        }
    }
}
