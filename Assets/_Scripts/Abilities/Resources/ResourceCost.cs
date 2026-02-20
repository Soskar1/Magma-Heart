using System;
using System.Collections.Generic;

namespace MagmaHeart.Abilities.Resources
{
    [Serializable]
    public sealed class ResourceCost
    {
        private readonly Dictionary<ResourceId, int> m_amounts = new();

        public static ResourceCost Zero => new();

        public void Add(ResourceId resourceId, int amount)
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

        public int this[ResourceId resource]
        {
            get => m_amounts.TryGetValue(resource, out var amount) ? amount : 0;
        }

        public IEnumerable<(ResourceId ResourceId, int Amount)> GetAllCosts()
        {
            foreach (var keyValuePair in m_amounts)
            {
                yield return (keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
