using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Energy
    {
        private int m_maxEnergy; 
        private int m_currentEnergy;
        public event Action OnEnergyChanged;

        public int CurrentEnergy
        {
            get => m_currentEnergy;
            set
            {
                if (value < 0)
                {
                    Debug.LogWarning($"Energy must be positive. Tried to set: {value}");
                    value = 0;
                }

                m_currentEnergy = value;

                if (m_currentEnergy > m_maxEnergy)
                    m_currentEnergy = m_maxEnergy;

                OnEnergyChanged?.Invoke();
            }
        }

        public Energy(int maxEnergy)
        {
            m_maxEnergy = maxEnergy;
            m_currentEnergy = 0;
        }

        public void Reset() => CurrentEnergy = 0;
    }
}