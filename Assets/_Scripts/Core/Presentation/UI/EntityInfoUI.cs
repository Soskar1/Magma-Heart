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
        private MouseHover m_mouseHover;

        public void Initialize(MouseHover mouseHover)
        {
            m_mouseHover = mouseHover;
            m_mouseHover.OnMouseHover += HandleOnMouseHover;
        }

        private void OnDisable() => m_mouseHover.OnMouseHover -= HandleOnMouseHover;

        public void Show() => m_visual.gameObject.SetActive(true);
        public void Hide() => m_visual.gameObject.SetActive(false);

        private void DisplayEntityInfo(EntityModel model)
        {
            m_entityName.text = model.Name;
            m_healthPresenter.Register(model.Health);
        }

        private void HandleOnMouseHover(object obj, OnMouseHoverEventArgs args)
        {
            Entity entity = args.HoverResult.Entity;
            if (entity == null || entity.Model.IsPlayer)
            {
                m_healthPresenter.Unregister();
                Hide();
                return;
            }

            DisplayEntityInfo(entity.Model);
            Show();
        }
    }
}
