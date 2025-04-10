using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public interface IMeleeAttacker
    {
        public Collider2D HitCollider { get; }
        public Action OnAttackStarted { get; set; }
        public Action OnAttackEnded { get; set; }
        public void Attack();
    }
}