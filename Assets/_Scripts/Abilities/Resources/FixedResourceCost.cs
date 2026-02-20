using MagmaHeart.Abilities.Targeting;
using System;
using UnityEngine;

namespace MagmaHeart.Abilities.Resources
{
    [Serializable]
    public class FixedResourceCost : CostModule
    {
        [SerializeField] private ResourceId m_resourceId;
        [SerializeField] private int m_amount;

        public override ResourceCost ComputeCost(IGameWorld world, int executorId, AbilityTarget target)
        {
            var cost = new ResourceCost();
            
            if (m_resourceId != null && m_amount != 0)
                cost.Add(m_resourceId, m_amount);
            
            return cost;
        }
    }
}
