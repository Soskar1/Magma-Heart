using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class ActionMouseHoverStrategy : IMouseHoverStrategy
    {
        public MouseHoverResult Evaluate(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider == null)
                return MouseHoverResult.None;

            if (!hit.collider.TryGetComponent(out Entity entity))
                return MouseHoverResult.Entity;

            return MouseHoverResult.None;
        }
    }
}
