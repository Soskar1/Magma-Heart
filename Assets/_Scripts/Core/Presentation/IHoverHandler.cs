namespace MagmaHeart.Core.Presentation
{
    public interface IHoverHandler
    {
        public void HandleHoverResult(HoverResult hoverResult);
        public void ClearHover();
    }

    public interface IHoverHandler<T> : IHoverHandler where T : HoverResult
    {
        public void HandleHoverResult(T hoverResult);
    }
}