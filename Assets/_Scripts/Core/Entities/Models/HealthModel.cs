using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class HealthModel
    {
        private float m_currentHealth;
        private float m_maxHealth;

        public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

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

        public HealthModel(float maxHealth)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
        }

        public HealthModel DeepCopy()
        {
            HealthModel copy = new HealthModel(MaxHealth)
            {
                CurrentHealth = CurrentHealth
            };
            return copy;
        }
    }
}
