namespace MagmaHeart.Core.Input.Mouse
{
    public interface IHoverHandler : IHoverResultVisitor
    {
        public void ClearHover();
    }
}