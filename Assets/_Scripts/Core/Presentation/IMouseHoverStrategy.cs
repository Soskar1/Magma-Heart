using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public interface IMouseHoverStrategy
    {
        public MouseHoverResult Evaluate(Vector2 worldPosition);
    }
}
