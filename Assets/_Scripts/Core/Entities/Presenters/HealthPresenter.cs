using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        private HealthModel m_healthModel;

        public void Initialize(Player player)
        {
            m_healthModel = player.Health;
            m_healthModel.OnHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            m_healthModel.OnHealthChanged -= UpdateHealthBar;
        }

        public void UpdateHealthBar(object obj, OnHealthChangedEventArgs e)
        {
            m_healthBar.fillAmount = e.CurrentHealth / e.MaxHealth;
        }
    }
}