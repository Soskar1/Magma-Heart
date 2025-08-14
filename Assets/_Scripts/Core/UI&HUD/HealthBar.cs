using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        private Health m_playerHealth;

        public void Initialize(Player player)
        {
            m_playerHealth = player.Health;
            m_playerHealth.OnCurrentHealthChanged += UpdateHealthBar;
            m_playerHealth.OnMaxHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            m_playerHealth.OnCurrentHealthChanged -= UpdateHealthBar;
            m_playerHealth.OnMaxHealthChanged -= UpdateHealthBar;
        }

        public void UpdateHealthBar(object obj, EventArgs e)
        {
            m_healthBar.fillAmount = m_playerHealth.CurrentHealth / m_playerHealth.MaxHealth;
        }
    }
}