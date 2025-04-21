using MagmaHeart.Core.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        private Health m_playerHealth;

        public void Initialize(Entity playerEntity)
        {
            m_playerHealth = playerEntity.Health;
            m_playerHealth.OnTakeDamage += UpdateHealthBar;
            m_playerHealth.OnMaxHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            m_playerHealth.OnTakeDamage -= UpdateHealthBar;
            m_playerHealth.OnMaxHealthChanged -= UpdateHealthBar;
        }

        public void UpdateHealthBar()
        {
            m_healthBar.fillAmount = m_playerHealth.CurrentHealth / m_playerHealth.MaxHealth;
        }
    }
}