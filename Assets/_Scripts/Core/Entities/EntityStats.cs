using MagmaHeart.Core.Abilities;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [System.Serializable]
    public class EntityStats
    {
        [SerializeField] private float m_maxHealth = 5;
        [SerializeField] private int m_maxEnergy = 7;
        [SerializeField] private int m_energyRegenerationPerTurn = 5;
        [SerializeField] private int m_strength = 0;
        [SerializeField] private int m_speed = 0;

        public EntityStats(float maxHealth = 5, int maxEnergy = 7, int energyRegenerationPerTurn = 5, int strength = 0, int speed = 0)
        {
            m_maxHealth = maxHealth;
            m_maxEnergy = maxEnergy;
            m_energyRegenerationPerTurn = energyRegenerationPerTurn;
            m_strength = strength;
            m_speed = speed;
        }

        public float MaxHealth => m_maxHealth;
        public int MaxEnergy => m_maxEnergy;
        public int EnergyRegenerationPerTurn => m_energyRegenerationPerTurn;
        public int Strength => m_strength;
        public int Speed => m_speed;

        // Note: used for difficulty scaling. Possibly will need to remove this in the future
        public void IncreaseHealth(int amount) => m_maxHealth += amount;
        public void IncreaseStregth(int amount) => m_strength += amount;
        public void IncreaseSpeed(int amount) => m_speed += amount;

        
    }
}