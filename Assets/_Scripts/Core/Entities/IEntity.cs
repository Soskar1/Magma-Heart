using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public interface IEntity
    {
        public Vector2 Position { get; }
        public Vector2 CurrentMovementDirection { get; }
        public Health Health { get; }
        public void ApplyMovement(Vector2 direction);
        public void Attack();
        public void Hit(in float damage);
        public Action OnAttack { get; set; }
    }
}