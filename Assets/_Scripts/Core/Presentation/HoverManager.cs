namespace MagmaHeart.Core.Presentation
{
    public class HoverManager
    {
        private readonly MouseHover m_mouseHover;

        private IHoverHandler m_handler;

        public HoverManager(MouseHover mouseHover)
        {
            m_mouseHover = mouseHover;
            m_mouseHover.OnHoverWorldPosition += HandleMouseHover;
        }

        public void Disable() => m_mouseHover.OnHoverWorldPosition -= HandleMouseHover;

        public void SetHandler(IHoverHandler handler) => m_handler = handler;
        private void HandleMouseHover(object obj, OnMouseHoverEventArgs args) => m_handler?.HandleHover(args.WorldPosition);
    }
}