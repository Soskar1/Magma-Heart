using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    [System.Serializable]
    public class SpeedStatModifier : IStatModifier
    {
        [SerializeField] private int m_additionalSpeed;

        public void Apply(EntityModel entity) => entity.Speed.CurrentSpeed += m_additionalSpeed;
        public void Revert(EntityModel entity) => entity.Speed.CurrentSpeed -= m_additionalSpeed;
    }
}
