using System;

namespace MagmaHeart.Core.Entities
{
    public class Health
    {
        private float m_currentHealth;
        private float m_maxHealth;

        public Action OnTakeDamage;
        public Action OnMaxHealthChanged;
        public Action OnCurrentHealthChanged;
        public Action OnDeath;

        public float CurrentHealth => m_currentHealth;
        public float MaxHealth => m_maxHealth;

        public Health(float maxHealth)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
        }

        public void Reset() => m_currentHealth = m_maxHealth;

        public void TakeDamage(in float damage)
        {
            m_currentHealth -= damage;
            OnTakeDamage?.Invoke();
            OnCurrentHealthChanged?.Invoke();

            if (m_currentHealth <= 0)
                OnDeath?.Invoke();
        }

        public void IncreaseMaxHealth(float amount) 
        {
            m_maxHealth += amount;
            OnMaxHealthChanged?.Invoke();
        }

        public void SetCurrentHealth(float newHealth)
        {
            if (newHealth > m_maxHealth)
                newHealth = m_maxHealth;

            m_currentHealth = newHealth;
            OnCurrentHealthChanged?.Invoke();
        }
    }
}
