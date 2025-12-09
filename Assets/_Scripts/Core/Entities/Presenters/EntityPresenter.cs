using MagmaHeart.Core.CombatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EntityPresenter : MonoBehaviour
    {
        [SerializeField] private HealthPresenter m_healthPresenter;
        [SerializeField] private Image m_portrait;
        [SerializeField] private Image m_currentTurnVisual;
        private EntityModel m_entity;
        private Battle m_battle;

        public void Initialize(EntityModel entity, Battle battle)
        {
            m_entity = entity;
            m_battle = battle;
            m_healthPresenter.Register(entity.Health);
            
            if (entity.Data.PortraitImage != null)
                m_portrait.sprite = entity.Data.PortraitImage;

            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        private void OnDisable() => m_battle.OnTurnSwitched -= HandleOnTurnSwitched;

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model == m_entity)
                m_currentTurnVisual.enabled = true;
            else
                m_currentTurnVisual.enabled = false;
        }

        // TODO: mouse hover must trigger entity outline and entity ui info popup
    }
}
