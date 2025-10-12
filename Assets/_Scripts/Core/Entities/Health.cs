using System;

namespace MagmaHeart.Core.Entities
{
    public class Health
    {
        private float m_currentHealth;
        private float m_maxHealth;

        public event EventHandler OnTakeDamage;
        public event EventHandler OnMaxHealthChanged;
        public event EventHandler OnCurrentHealthChanged;
        public event EventHandler OnDeath;

        public float CurrentHealth
        {
            get => m_currentHealth;
            set
            {
                if (value > m_maxHealth)
                    value = m_maxHealth;

                m_currentHealth = value;
                OnCurrentHealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public float MaxHealth
        {
            get => m_maxHealth;
            set
            {
                m_maxHealth = value;

                OnMaxHealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Health(float maxHealth)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
        }

        public void Reset() => m_currentHealth = m_maxHealth;

        public void TakeDamage(in float damage)
        {
            m_currentHealth -= damage;
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
            OnCurrentHealthChanged?.Invoke(this, EventArgs.Empty);

            if (m_currentHealth <= 0)
                OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
