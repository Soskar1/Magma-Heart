using MagmaHeart.Abilities;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Models
{
    public class EnergyModel : IParameter
    {
        private int m_currentEnergy;
        private int m_maxEnergy;
        private int m_energyRegenerationPerTurn;

        public event EventHandler<OnParameterValueChangedEventArgs> OnParameterValueChanged;

        public ParameterId Id { get; init; }
        public float CurrentValue => CurrentEnergy;

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

                OnParameterValueChangedEventArgs args = new OnParameterValueChangedEventArgs(Id, CurrentEnergy);
                OnParameterValueChanged?.Invoke(this, args);
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

        public EnergyModel(ParameterId parameterId, int maxEnergy, int energyRegenerationPerTurn)
        {
            Id = parameterId;

            MaxEnergy = maxEnergy;
            EnergyRegenerationPerTurn = energyRegenerationPerTurn;
            m_currentEnergy = 0;
        }

        public void Reset() => CurrentEnergy = 0;

        public EnergyModel DeepCopy()
        {
            EnergyModel copy = new EnergyModel(Id, MaxEnergy, EnergyRegenerationPerTurn)
            {
                CurrentEnergy = CurrentEnergy
            };
            return copy;
        }

        public void SetValue(float value) => CurrentEnergy = (int)value;
    }
}