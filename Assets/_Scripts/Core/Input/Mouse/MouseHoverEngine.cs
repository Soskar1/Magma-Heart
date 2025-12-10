using System;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class MouseHoverEngine
    {
        private readonly MouseListener m_listener;
        private MouseHoverStrategy m_activeHoverStrategyChain;
        private IHoverHandler m_activeHandler;
        private Vector2 m_currentMousePosition;

        public event EventHandler<OnMouseHoverEventArgs> OnHover;

        public MouseHoverEngine(MouseListener listener)
        {
            m_listener = listener;
            m_listener.OnMouseWorldPositionChanged += HandleMousePositionChanged;
        }

        public void Disable() => m_listener.OnMouseWorldPositionChanged -= HandleMousePositionChanged;

        public void SetStrategyChain(MouseHoverStrategy strategy) => m_activeHoverStrategyChain = strategy;

        public void SetHandler(IHoverHandler handler)
        {
            m_activeHandler?.ClearHover();
            m_activeHandler = handler;

            Hover(m_currentMousePosition);
        }

        private void HandleMousePositionChanged(object obj, OnMouseWorldPositionChangedEventArgs args)
        {
            m_currentMousePosition = args.WorldPosition;
            Hover(m_currentMousePosition);
        }

        private void Hover(Vector2 worldPosition)
        {
            HoverResult result = m_activeHoverStrategyChain.Hover(worldPosition);

            OnHover?.Invoke(this, new OnMouseHoverEventArgs(result));

            if (result != null && m_activeHandler != null)
                result.Accept(m_activeHandler);
        }
    }
}
