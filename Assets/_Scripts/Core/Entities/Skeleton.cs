using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Skeleton : MonoBehaviour, IHittable
    {
        [Header("Health")]
        [SerializeField] private float m_maxHealth;
        private Health m_health;

        [Header("Field of view")]
        [SerializeField] private float m_fieldOfViewRadius;
        [SerializeField] private int m_amountOfRaycasts;
        private FieldOfView m_fieldOfView;

        private RigidbodyMovement m_movement;
        private Rigidbody2D m_rigidbody;
        
        private AnimationPlayer m_animationPlayer;

        [SerializeField] private bool m_facingRight;
        [SerializeField] private float m_triggerAttackDistance;
        private Player m_playerToChase;
        private Facing m_facing;
        private bool m_isAttacking;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_health = new Health(m_maxHealth);
            m_fieldOfView = new FieldOfView(m_fieldOfViewRadius, m_amountOfRaycasts, transform);
            m_movement = GetComponent<RigidbodyMovement>();
            m_facing = new Facing(transform, m_facingRight);

            Animator animator = GetComponent<Animator>();
            int m_idleAnimationID = Animator.StringToHash("Idle");
            int m_walkAnimationID = Animator.StringToHash("Walk");
            int m_attackAnimationID = Animator.StringToHash("Attack");

            m_animationPlayer = new AnimationPlayer(animator, m_idleAnimationID, () => {
                if (m_isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    m_isAttacking = false;

                if (m_isAttacking)
                    return m_attackAnimationID;

                if (m_playerToChase != null)
                    return m_walkAnimationID;

                return m_idleAnimationID;
            });
        }

        private void Start()
        {
            m_health.OnTakeDamage += () => Debug.Log("Took damage");
        }

        private void Update()
        {
            if (m_playerToChase == null)
                m_playerToChase = m_fieldOfView.FindPlayer();

            if (m_playerToChase != null && Vector2.Distance(transform.position, m_playerToChase.transform.position) < m_triggerAttackDistance && !m_isAttacking)
            {
                m_isAttacking = true;
                m_rigidbody.linearVelocity = Vector2.zero;
            }

            m_animationPlayer.PlayAnimations();
        }

        private void FixedUpdate()
        {
            if (m_playerToChase == null)
                return;

            if (!m_isAttacking)
            {
                Vector2 directionToMove = (m_playerToChase.transform.position - transform.position).normalized;

                m_facing.TryUpdateFacing(directionToMove.x);

                m_movement.Move(directionToMove);
            }
        }

        public void Hit(in float damage) => m_health.TakeDamage(damage);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector2 raycast = Vector2.right;

            for (int i = 0; i < m_amountOfRaycasts; ++i)
            {
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + raycast * m_fieldOfViewRadius);
                raycast = raycast.Rotate(360.0f /  m_amountOfRaycasts);
            }
        }
    }
}