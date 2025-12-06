using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class MouseHover
    {
        private readonly MousePositionListener m_mousePositionListener;
        private readonly PlayerTurnContext m_turnContext;
        private readonly RaycastHoverHandler m_raycastHoverHandler;
        private readonly CombatHoverHandler m_combatHoverHandler;
        private IHoverHandler m_currentHandler;
        private Vector2 m_currentMousePosition;

        private readonly Battle m_battle;

        public MouseHover(MousePositionListener mousePositionListener, PlayerTurnContext playerTurnContext, Battle battle)
        {
            m_mousePositionListener = mousePositionListener;
            m_turnContext = playerTurnContext;
            m_raycastHoverHandler = new RaycastHoverHandler();
            m_combatHoverHandler = new CombatHoverHandler(playerTurnContext);

            m_turnContext.OnCombatActionExecuted += TriggerHover;
            m_mousePositionListener.OnMouseWorldPositionChanged += HandleOnMousePositionChanged;

            m_battle = battle;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        public void Disable()
        {
            m_mousePositionListener.OnMouseWorldPositionChanged -= HandleOnMousePositionChanged;
            m_turnContext.OnCombatActionExecuted -= TriggerHover;
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        public void UseRaycastHover() => m_currentHandler = m_raycastHoverHandler;
        public void UseCombatHover() => m_currentHandler = m_combatHoverHandler;
        private void HandleOnMousePositionChanged(object obj, OnMouseWorldPositionChangedEventArgs args)
        {
            m_currentMousePosition = args.WorldPosition;
            TriggerHover();
        }

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_currentHandler.ClearHover();

            if (args.CurrentEntity.Model.IsPlayer)
                UseCombatHover();
            else
                UseRaycastHover();
        }

        private void TriggerHover() => m_currentHandler?.HandleHover(m_currentMousePosition);
    }
}