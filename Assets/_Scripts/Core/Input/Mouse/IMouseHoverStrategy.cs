using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public interface IMouseHoverStrategy
    {
        public HoverResult Hover(Vector2 worldPosition);
    }
}
