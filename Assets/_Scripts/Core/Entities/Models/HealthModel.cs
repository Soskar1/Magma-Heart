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

                OnHealthChangedEventArgs args = new OnHealthChangedEventArgs(CurrentHealth, MaxHealth);
                OnHealthChanged?.Invoke(this, args);
            }
        }

        public HealthModel(float maxHealth)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
        }
    }
}
