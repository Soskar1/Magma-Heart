using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public interface IEntity
    {
        public Vector2 Position { get; }
        public Health Health { get; }
        public void ApplyMovement(Vector2 direction);
        public void Attack();
        public void Hit(in float damage);
    }
}