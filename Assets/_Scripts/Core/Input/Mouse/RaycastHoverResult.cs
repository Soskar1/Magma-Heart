using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Input.Mouse
{
    public record RaycastHoverResult(Entity Entity) : HoverResult
    {
        public override void Accept(IHoverResultVisitor visitor) => visitor.Visit(this);
    }
}
