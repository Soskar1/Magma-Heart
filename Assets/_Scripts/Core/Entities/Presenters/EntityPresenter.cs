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
        private Battle m_battle;

        public Entity Entity { get; private set; }

        public void Initialize(Entity entity, Battle battle)
        {
            Entity = entity;
            m_battle = battle;
            m_healthPresenter.Register(entity.Health);

            if (entity.Model.Data.PortraitImage != null)
                m_portrait.sprite = entity.Model.Data.PortraitImage;

            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        private void OnDisable()
        {
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model == Entity.Model)
                m_currentTurnVisual.enabled = true;
            else
                m_currentTurnVisual.enabled = false;
        }
    }
}
