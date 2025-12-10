namespace MagmaHeart.Core.Presentation
{
    public abstract record HoverResult
    {
        public abstract void Accept(IHoverResultVisitor visitor);
    }
}
