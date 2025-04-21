using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    // Increases attack speed
    public class EnergyDrink : Artifact
    {
        [SerializeField] private float m_attackSpeedAmount;
        public float AttackSpeedAmount => m_attackSpeedAmount;
    }
}