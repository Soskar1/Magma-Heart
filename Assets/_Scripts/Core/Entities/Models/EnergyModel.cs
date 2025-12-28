using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Models
{
    public class EnergyModel
    {
        private int m_currentEnergy;

        public event EventHandler<OnEnergyChangedEventArgs> OnEnergyChanged;

        public int MaxEnergy { get; init; }

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

        public EnergyModel(int maxEnergy)
        {
            MaxEnergy = maxEnergy;
            m_currentEnergy = 0;
        }

        public void Reset() => CurrentEnergy = 0;
    }
}