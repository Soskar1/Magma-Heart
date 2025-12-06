using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.PlayableCharacters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private TextMeshProUGUI m_text;
        private HealthModel m_healthModel;

        public void Initialize(Player player)
        {
            m_healthModel = player.Health;
            m_healthModel.OnHealthChanged += HandleOnHealthChangedEvent;

            UpdateHealthBar(m_healthModel.CurrentHealth, m_healthModel.MaxHealth);
        }

        private void OnDisable() => m_healthModel.OnHealthChanged -= HandleOnHealthChangedEvent;

        private void HandleOnHealthChangedEvent(object obj, OnHealthChangedEventArgs e) => UpdateHealthBar(e.CurrentHealth, e.MaxHealth);

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            m_healthBar.fillAmount = currentHealth / maxHealth;
            m_text.text = $"{currentHealth}/{maxHealth}";
        }
    }
}