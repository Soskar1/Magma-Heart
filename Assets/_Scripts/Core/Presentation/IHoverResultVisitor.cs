namespace MagmaHeart.Core.Presentation
{
    public interface IHoverResultVisitor
    {
        void Visit(UIHoverResult result);
        void Visit(CombatHoverResult result);
        void Visit(RaycastHoverResult result);
    }
}
