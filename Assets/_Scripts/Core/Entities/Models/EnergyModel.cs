using MagmaHeart.Abilities.Resources;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Models
{
    public class EnergyModel
    {
        private int m_currentEnergy;
        private int m_maxEnergy;
        private int m_energyRegenerationPerTurn;

        public event EventHandler<OnEnergyChangedEventArgs> OnEnergyChanged;

        public ResourceId ResourceId { get; init; }

        public int MaxEnergy
        { 
            get => m_maxEnergy;
            set
            {
                m_maxEnergy = value;

                if (m_currentEnergy > m_maxEnergy)
                    m_currentEnergy = m_maxEnergy;
            }
        }

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

                if (m_currentEnergy > MaxEnergy)
                    m_currentEnergy = MaxEnergy;

                OnEnergyChangedEventArgs args = new OnEnergyChangedEventArgs(CurrentEnergy, MaxEnergy);
                OnEnergyChanged?.Invoke(this, args);
            }
        }

        public int EnergyRegenerationPerTurn
        {
            get => m_energyRegenerationPerTurn;
            set
            {
                if (value < 0)
                    value = 0;

                m_energyRegenerationPerTurn = value;
            }
        }

        public EnergyModel(ResourceId resourceId, int maxEnergy, int energyRegenerationPerTurn)
        {
            ResourceId = resourceId;

            MaxEnergy = maxEnergy;
            EnergyRegenerationPerTurn = energyRegenerationPerTurn;
            m_currentEnergy = 0;
        }

        public void Reset() => CurrentEnergy = 0;

        public EnergyModel DeepCopy()
        {
            EnergyModel copy = new EnergyModel(ResourceId, MaxEnergy, EnergyRegenerationPerTurn)
            {
                CurrentEnergy = CurrentEnergy
            };
            return copy;
        }
    }
}