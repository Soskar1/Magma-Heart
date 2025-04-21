using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    // Increases movement speed
    public class Boots : Artifact
    {
        [SerializeField] private float m_speedAmount;
        public float SpeedAmount => m_speedAmount;
    }
}