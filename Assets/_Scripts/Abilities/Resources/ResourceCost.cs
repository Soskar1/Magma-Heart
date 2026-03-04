using System;
using System.Collections.Generic;

namespace MagmaHeart.Abilities.Resources
{
    [Serializable]
    public sealed class ResourceCost
    {
        private readonly Dictionary<ParameterId, int> m_amounts = new();

        public static ResourceCost Zero => new();

        public void Add(ParameterId resourceId, int amount)
        {
            if (amount == 0)
                return;

            if (m_amounts.ContainsKey(resourceId))
                m_amounts[resourceId] += amount;
            else
                m_amounts[resourceId] = amount;
        }

        public void Add(ResourceCost other)
        {
            foreach (var keyValuePair in other.m_amounts)
                Add(keyValuePair.Key, keyValuePair.Value);
        }

        public IEnumerable<(ParameterId Id, int Amount)> GetAllCosts()
        {
            foreach (var keyValuePair in m_amounts)
            {
                yield return (keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
