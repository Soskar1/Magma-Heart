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

        private bool m_isAttacking = false;
        private AnimationPlayer m_animationPlayer;

        public Health Health => m_health;
        public Vector2 Position => transform.position;

        private Vector2 m_currentMovementDirection;

        private void Awake() 
        {
            m_movement = GetComponent<RigidbodyMovement>();
            m_health = new Health(m_maxHealth);
            m_facing = new Facing(transform, m_facingRight);

            m_currentMovementDirection = new Vector2();

            int idleAnimationID = Animator.StringToHash("Idle");
            int runAnimationID = Animator.StringToHash("Run");
            int attackAnimationID = Animator.StringToHash("Attack");

            Animator animator = GetComponent<Animator>();
            m_animationPlayer = new AnimationPlayer(animator, idleAnimationID, (currentAnimationState) => {
                if (m_isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && currentAnimationState == attackAnimationID)
                    m_isAttacking = false;

                if (m_isAttacking)
                    return attackAnimationID;
                
                if (Mathf.Abs(m_currentMovementDirection.magnitude) > 0)
                    return runAnimationID;
                
                return idleAnimationID;
            });
        }

        private void Update()
        {
            m_facing.TryUpdateFacing(m_currentMovementDirection.x);
            m_animationPlayer.PlayAnimations();
        }

        private void FixedUpdate() => m_movement.Move(m_currentMovementDirection);

        public void Hit(in float damage) => m_health.TakeDamage(damage);

        public void ApplyMovement(Vector2 direction) => m_currentMovementDirection = direction;
        
        public void Attack() => m_isAttacking = true;
    }
}
