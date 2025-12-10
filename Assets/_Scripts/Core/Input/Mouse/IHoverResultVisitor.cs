namespace MagmaHeart.Core.Input.Mouse
{
    public interface IHoverResultVisitor
    {
        void Visit(UIHoverResult result);
        void Visit(CombatHoverResult result);
        void Visit(RaycastHoverResult result);
    }
}
