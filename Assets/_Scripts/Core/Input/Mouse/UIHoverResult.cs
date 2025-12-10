using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public record UIHoverResult(GameObject UIElement) : HoverResult
    {
        public override void Accept(IHoverResultVisitor visitor) => visitor.Visit(this);
    }
}
