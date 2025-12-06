using MagmaHeart.Core.CombatSystem;
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
        private Battle m_battle;
        private EntityModel m_currentDisplayedEntity;

        public void Initialize(MouseHover mouseHover, Battle battle)
        {
            m_mouseHover = mouseHover;
            m_battle = battle;

            m_mouseHover.OnMouseHover += HandleOnMouseHover;
            m_battle.OnEntityDied += HandleOnEntityDied;
        }

        private void OnDisable()
        {
            m_mouseHover.OnMouseHover -= HandleOnMouseHover;
            m_battle.OnEntityDied -= HandleOnEntityDied;
        }
        
        public void Show() => m_visual.gameObject.SetActive(true);
        
        public void Hide()
        {
            m_healthPresenter.Unregister();

            m_visual.gameObject.SetActive(false);
            
            m_currentDisplayedEntity = null;
        }

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
                Hide();
                return;
            }

            m_currentDisplayedEntity = entity.Model;
            DisplayEntityInfo(entity.Model);
            Show();
        }

        private void HandleOnEntityDied(object obj, OnEntityDiedEventArgs args)
        {
            if (args.Model == m_currentDisplayedEntity)
                Hide();
        }
    }
}
