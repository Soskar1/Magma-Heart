using MagmaHeart.Abilities;
using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class HealthModel : IParameter
    {
        private float m_currentHealth;
        private float m_maxHealth;

        public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

        public ParameterId Id { get; init; }
        public float CurrentValue => CurrentHealth;

        public float CurrentHealth
        {
            get => m_currentHealth;
            set
            {
                if (value > m_maxHealth)
                    value = m_maxHealth;

                if (value < 0)
                    value = 0;

                m_currentHealth = value;

                OnHealthChangedEventArgs args = new OnHealthChangedEventArgs(CurrentHealth, MaxHealth);
                OnHealthChanged?.Invoke(this, args);
            }
        }

        public float MaxHealth
        {
            get => m_maxHealth;
            set
            {
                m_maxHealth = value;

                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;

                OnHealthChangedEventArgs args = new OnHealthChangedEventArgs(CurrentHealth, MaxHealth);
                OnHealthChanged?.Invoke(this, args);
            }
        }

        public HealthModel(float maxHealth, ParameterId id)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
            Id = id;
        }

        public HealthModel DeepCopy()
        {
            HealthModel copy = new HealthModel(MaxHealth, Id)
            {
                CurrentHealth = CurrentHealth
            };
            return copy;
        }

        public void SetValue(float value) => CurrentHealth = value;
    }
}
