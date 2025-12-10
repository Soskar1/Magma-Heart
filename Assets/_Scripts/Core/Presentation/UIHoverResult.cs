using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public record UIHoverResult(GameObject UIElement) : HoverResult
    {
        public override void Accept(IHoverResultVisitor visitor) => visitor.Visit(this);
    }
}
