using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    // Increases max health
    public class Apple : Artifact
    {
        [SerializeField] private float m_healthAmount;
        public float HealthAmount => m_healthAmount;
    }
}