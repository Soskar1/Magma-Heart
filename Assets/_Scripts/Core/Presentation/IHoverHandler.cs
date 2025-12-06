using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public interface IHoverHandler
    {
        public void HandleHover(Vector2 worldPosition);
        public void ClearHover();
    }
}