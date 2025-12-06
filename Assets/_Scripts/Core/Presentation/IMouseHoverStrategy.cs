using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public interface IMouseHoverStrategy
    {
        public HoverResult Hover(Vector2 worldPosition);
    }
}
