namespace MagmaHeart.Core.Input.Mouse
{
    public abstract record HoverResult
    {
        public abstract void Accept(IHoverResultVisitor visitor);
    }
}
