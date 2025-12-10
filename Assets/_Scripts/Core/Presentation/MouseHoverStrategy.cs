using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public abstract class MouseHoverStrategy
    {
        public MouseHoverStrategy Next { get; set; }

        public HoverResult Hover(Vector2 worldPosition)
        {
            HoverResult result = null;
            MouseHoverStrategy strategy = this;

            while (strategy != null)
            {
                result = strategy.TryHover(worldPosition);

                if (result != null)
                    break;

                strategy = strategy.Next;
            }

            return result;
        }

        protected abstract HoverResult TryHover(Vector2 worldPosition);
    }
}
