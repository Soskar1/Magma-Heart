using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(RigidbodyMovement))]
    [RequireComponent(typeof(Animator))]
    public class Warrior : MonoBehaviour, IEntity
    {
        [SerializeField] private float m_maxHealth;
        [SerializeField] private bool m_facingRight;
        private RigidbodyMovement m_movement;
        private Health m_health;
        private Facing m_facing;

        private AnimationPlayer m_animationPlayer;
        private Vector2 m_currentMovementDirection;

        private Action m_onAttack;
        public Health Health => m_health;
        public Vector2 Position => transform.position;
        public Vector2 CurrentMovementDirection => m_currentMovementDirection;
        public Action OnAttack { get => m_onAttack; set => m_onAttack = value; }

        private void Awake() 
        {
            m_movement = GetComponent<RigidbodyMovement>();
            m_health = new Health(m_maxHealth);
            m_facing = new Facing(transform, m_facingRight);

            m_currentMovementDirection = new Vector2();

            Animator animator = GetComponent<Animator>();
            m_animationPlayer = new WarriorAnimationPlayer(animator, this);
        }

        private void Update()
        {
            m_facing.TryUpdateFacing(m_currentMovementDirection.x);
            m_animationPlayer.PlayAnimations();
        }

        private void FixedUpdate() => m_movement.Move(m_currentMovementDirection);

        public void Hit(in float damage) => m_health.TakeDamage(damage);

        public void ApplyMovement(Vector2 direction) => m_currentMovementDirection = direction;
        
        public void Attack() => OnAttack?.Invoke();
    }
}
