using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public interface IMovement
    {
        public Vector2 CurrentMovementDirection { get; }
        public void Move(Vector2 direction);
    }
}