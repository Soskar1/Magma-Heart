using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Cost
{
    [Serializable]
    public class ResourcePerNTilesCost : CostModule
    {
        [SerializeField] private ParameterDatabase m_database;
        [SerializeField] private int m_tilesPerResource;

        public override ResourceCost ComputeCost(IGameWorld world, int executorId, AbilityTarget target)
        {
            IParameter speed = world.GetParameter(executorId, m_database.Speed);
            int totalTilesPerResource = m_tilesPerResource + (int)speed.CurrentValue;

            if (totalTilesPerResource <= 0)
                return ResourceCost.Zero;

            int steps = 0;

            if (target.Kind == TargetKind.Path && target.Path != null && target.Path.Count > 0)
                steps = Math.Max(0, target.Path.Count - 1);
            else
                return ResourceCost.Zero;

            int amount = Mathf.CeilToInt(steps / (float)totalTilesPerResource);
            ResourceCost cost = new ResourceCost();
            cost.Add(m_database.Energy, amount);
            
            return cost;
        }
    }
}
