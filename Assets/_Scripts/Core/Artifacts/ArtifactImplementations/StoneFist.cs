using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    // Increases attack damage
    public class StoneFist : Artifact
    {
        [SerializeField] private float m_damageAmount;
        public float DamageAmount => m_damageAmount;
    }
}