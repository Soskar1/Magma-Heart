using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Presentation
{
    public class MouseHover
    {
        private readonly MouseListener m_mousePositionListener;
        private readonly PlayerTurnContext m_turnContext;
        private Vector2 m_currentMousePosition;

        private MouseHoverStrategy m_currentHoverStrategy;
        private readonly RaycastMouseHoverStrategy m_raycastHoverStrategy;
        private readonly UIMouseHoverStrategy m_uiHoverStrategy;
        private readonly MouseHoverStrategy m_combatHoverStrategy;

        private IHoverHandler m_currentHandler;
        private readonly ActionHoverHandler m_actionHandler;
        private readonly CombatHoverHandler m_combatHandler;

        private readonly Battle m_battle;

        public event EventHandler<OnMouseHoverEventArgs> OnMouseHover;

        public MouseHover(MouseListener mousePositionListener, PlayerTurnContext playerTurnContext, Battle battle, GraphicRaycaster graphicRaycaster)
        {
            m_mousePositionListener = mousePositionListener;
            m_turnContext = playerTurnContext;
            
            m_raycastHoverStrategy = new RaycastMouseHoverStrategy();
            m_uiHoverStrategy = new UIMouseHoverStrategy(graphicRaycaster);

            m_combatHoverStrategy = m_uiHoverStrategy;
            m_combatHoverStrategy.Next = new TileMouseHoverStrategy(playerTurnContext);

            m_actionHandler = new ActionHoverHandler();
            m_combatHandler = new CombatHoverHandler(playerTurnContext);

            m_mousePositionListener.OnMouseWorldPositionChanged += HandleOnMousePositionChanged;
            m_turnContext.OnCombatActionExecutionStarted += HandleOnCombatActionExecutionStarted;
            m_turnContext.OnCombatActionExecuted += HandleOnCombatActionExecuted;

            m_battle = battle;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        public void Disable()
        {
            m_mousePositionListener.OnMouseWorldPositionChanged -= HandleOnMousePositionChanged;
            m_turnContext.OnCombatActionExecutionStarted -= HandleOnCombatActionExecutionStarted;
            m_turnContext.OnCombatActionExecuted -= HandleOnCombatActionExecuted;
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        public void UseRaycastHover()
        {
            m_currentHandler?.ClearHover();
            m_currentHoverStrategy = m_raycastHoverStrategy;
            m_currentHandler = m_actionHandler;
        }

        public void UseTileHover()
        {
            m_currentHandler?.ClearHover();
            m_currentHoverStrategy = m_combatHoverStrategy;
            m_currentHandler = m_combatHandler;
        }

        private void HandleOnMousePositionChanged(object obj, OnMouseWorldPositionChangedEventArgs args)
        {
            m_currentMousePosition = args.WorldPosition;
            Hover();
        }

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                UseTileHover();
            else
                UseRaycastHover();
        }

        private void Hover()
        {
            HoverResult hoverResult = m_currentHoverStrategy?.Hover(m_currentMousePosition);
            
            OnMouseHoverEventArgs args = new OnMouseHoverEventArgs(hoverResult);
            OnMouseHover?.Invoke(this, args);

            if (m_currentHandler != null && hoverResult != null)
                hoverResult.Accept(m_currentHandler);
        }

        private void HandleOnCombatActionExecutionStarted()
        {
            UseRaycastHover();
            Hover();
        }

        private void HandleOnCombatActionExecuted()
        {
            UseTileHover();
            Hover();
        }
    }
}