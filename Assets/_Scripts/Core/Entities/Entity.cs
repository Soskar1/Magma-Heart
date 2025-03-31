using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private float m_maxHealth;
        [SerializeField] private bool m_facingRight;
        private Health m_health;

        private RigidbodyMovement m_movement;

        private AnimationPlayer m_animationPlayer;

        private Facing m_facing;

        private Action m_onAttack;

        private Vector2 m_currentMovementDirection;
        public Health Health => m_health;
        public Vector2 Position => transform.position;
        public Vector2 CurrentMovementDirection => m_currentMovementDirection;
        public Action OnAttack { get => m_onAttack; set => m_onAttack = value; }

        private void Awake()
        {
            m_health = new Health(m_maxHealth);
            m_movement = GetComponent<RigidbodyMovement>();
            m_facing = new Facing(transform, m_facingRight);

            m_animationPlayer = GetComponent<AnimationPlayer>();
        }

        private void Update()
        {
            m_animationPlayer.PlayAnimations();
        }

        private void FixedUpdate()
        {
            m_facing.TryUpdateFacing(m_currentMovementDirection.x);
            m_movement.Move(m_currentMovementDirection);
        }

        public void Hit(in float damage) => m_health.TakeDamage(damage);

        public void ApplyMovement(Vector2 direction) => m_currentMovementDirection = direction;
        public void Attack() => OnAttack?.Invoke();
    }
}