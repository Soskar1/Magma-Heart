using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class RaycastMouseHoverStrategy : IMouseHoverStrategy
    {
        public HoverResult Hover(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider != null && hit.collider.TryGetComponent(out Entity entity))
                return new HoverResult(entity);

            return new HoverResult(null);
        }
    }
}
