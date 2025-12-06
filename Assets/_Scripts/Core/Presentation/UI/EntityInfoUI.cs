using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class EntityInfoUI : MonoBehaviour, IDisplayable
    {
        [SerializeField] private HealthPresenter m_healthPresenter;
        [SerializeField] private GameObject m_visual;
        [SerializeField] private TextMeshProUGUI m_entityName;

        public void Show() => m_visual.gameObject.SetActive(true);
        public void Hide() => m_visual.gameObject.SetActive(false);

        public void DisplayEntityInfo(EntityModel model)
        {
            m_entityName.text = model.Name;
            m_healthPresenter.UpdateHealthBar(model.Health.CurrentHealth, model.Health.MaxHealth);
            Show();
        }
    }
}
