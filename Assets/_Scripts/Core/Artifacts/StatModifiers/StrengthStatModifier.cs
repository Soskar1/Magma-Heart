using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    [System.Serializable]
    public class StrengthStatModifier : IStatModifier
    {
        [SerializeField] private int m_additionalStrength;

        public void Apply(EntityModel entity) => entity.Strength.CurrentStrength += m_additionalStrength;
        public void Revert(EntityModel entity) => entity.Strength.CurrentStrength -= m_additionalStrength;
    }
}
