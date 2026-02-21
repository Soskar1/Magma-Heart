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
        private Battle m_battle;
        private EntityModel m_currentDisplayedEntity;

        public void Initialize(Battle battle)
        {
            m_battle = battle;

            m_battle.OnEntityDied += HandleOnEntityDied;
        }

        private void OnDisable()
        {
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
            m_entityName.text = model.Data.Name;
            m_healthPresenter.Register(model.Health);
        }

        //private void HandleOnMouseHover(object obj, OnMouseHoverEventArgs args)
        //{
        //    EntityInfoExtractor extractor = new EntityInfoExtractor();
        //    args.HoverResult.Accept(extractor);
        //    EntityModel model = extractor.Model;

        //    if (model == null || model.IsPlayer)
        //    {
        //        Hide();
        //        return;
        //    }

        //    if (m_currentDisplayedEntity == model)
        //        return;

        //    m_currentDisplayedEntity = model;
        //    DisplayEntityInfo(model);
        //    Show();
        //}

        private void HandleOnEntityDied(object obj, OnEntityDiedEventArgs args)
        {
            if (args.DiedEntity.Model == m_currentDisplayedEntity)
                Hide();
        }
    }
}
