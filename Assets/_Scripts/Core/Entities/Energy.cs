using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Energy
    {
        private int m_maxEnergy;
        private int m_energyRegenerationPerTurn;
        private int m_currentEnergy;
        public Action OnEnergyChanged;

        public int CurrentEnergy
        {
            get => m_currentEnergy;
            private set
            {
                m_currentEnergy = value;

                if (m_currentEnergy > m_maxEnergy)
                    m_currentEnergy = m_maxEnergy;

                OnEnergyChanged?.Invoke();
            }
        }

        public Energy(int maxEnergy, int energyRegenerationPerTurn)
        {
            m_maxEnergy = maxEnergy;
            m_currentEnergy = 0;
            m_energyRegenerationPerTurn = energyRegenerationPerTurn;
        }

        public bool HasEnough(int energyToSpent) => energyToSpent <= CurrentEnergy;

        public void Spend(int amount)
        {
            if (!HasEnough(amount))
            {
                Debug.LogWarning($"Tried to spend {amount}, but entity has {CurrentEnergy}");
                return;
            }

            CurrentEnergy -= amount;
        }

        public void Regenerate() => CurrentEnergy += m_energyRegenerationPerTurn;
    }
}