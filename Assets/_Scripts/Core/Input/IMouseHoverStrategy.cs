using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public interface IMouseHoverStrategy
    {
        public void Evaluate(Vector2 worldPosition);
    }
}
