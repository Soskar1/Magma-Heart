using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class MeleeAttacker : MonoBehaviour, IMeleeAttacker
    {
        [SerializeField] private Collider2D m_hitCollider;

        public Collider2D HitCollider => m_hitCollider;

        public Action OnAttackStarted { get; set; }
        public Action OnAttackEnded { get; set; }

        public void Attack() => OnAttackStarted?.Invoke();
    }
}
