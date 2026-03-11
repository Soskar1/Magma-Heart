using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    [System.Serializable]
    public class HealthStatModifier : IStatModifier
    {
        [SerializeField] private float m_additionalHealth;

        public void Apply(EntityModel entity)
        {
            entity.Health.MaxHealth += m_additionalHealth;
            entity.Health.CurrentHealth += m_additionalHealth;
        }

        public void Revert(EntityModel entity) => entity.Health.MaxHealth -= m_additionalHealth;
    }
}
