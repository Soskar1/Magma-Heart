using System;

namespace MagmaHeart.Core.Entities
{
    public class Health
    {
        private float m_currentHealth;
        private float m_maxHealth;

        public Action OnTakeDamage;
        public Action OnDeath;

        public Health(float maxHealth)
        {
            m_maxHealth = maxHealth;
            m_currentHealth = maxHealth;
        }

        public void TakeDamage(in float damage)
        {
            m_currentHealth -= damage;
            OnTakeDamage?.Invoke();

            if (m_currentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
}
