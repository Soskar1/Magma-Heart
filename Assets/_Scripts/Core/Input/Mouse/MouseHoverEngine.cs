using System;

namespace MagmaHeart.Core.Input.Mouse
{
    public class MouseHoverEngine
    {
        private readonly MouseListener m_listener;
        private MouseHoverStrategy m_activeHoverStrategyChain;
        private IHoverHandler m_activeHandler;

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
        }

        private void HandleMousePositionChanged(object obj, OnMouseWorldPositionChangedEventArgs args)
        {
            HoverResult result = m_activeHoverStrategyChain.Hover(args.WorldPosition);

            OnHover?.Invoke(this, new OnMouseHoverEventArgs(result));

            if (result != null && m_activeHandler != null)
                result.Accept(m_activeHandler);
        }
    }
}
