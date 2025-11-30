using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class ActionMouseHoverStrategy : IMouseHoverStrategy
    {
        public void Evaluate(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider == null)
                return;

            if (!hit.collider.TryGetComponent(out Entity entity))
                return;
        }
    }
}
