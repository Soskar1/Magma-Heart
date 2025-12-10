namespace MagmaHeart.Core.Presentation
{
    public interface IHoverHandler : IHoverResultVisitor
    {
        public void ClearHover();
    }
}