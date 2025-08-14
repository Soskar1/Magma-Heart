using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [System.Serializable]
    public class EntityStats
    {
        [SerializeField] private float m_maxHealth = 5;
        [SerializeField] private int m_maxEnergy = 7;
        [SerializeField] private int m_energyRegenerationPerTurn = 5;

        public float MaxHealth => m_maxHealth;
        public int MaxEnergy => m_maxEnergy;
        public int EnergyRegenerationPerTurn => m_energyRegenerationPerTurn;
    }
}