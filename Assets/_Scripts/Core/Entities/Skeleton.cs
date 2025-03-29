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
        
        private Animator m_animator;
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_walkAnimationID = Animator.StringToHash("Walk");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");
        private int m_currentAnimationState;

        [SerializeField] private bool m_facingRight;
        [SerializeField] private float m_triggerAttackDistance;
        private Player m_playerToChase;
        private bool m_isAttacking;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_health = new Health(m_maxHealth);
            m_fieldOfView = new FieldOfView(m_fieldOfViewRadius, m_amountOfRaycasts, transform);
            m_movement = GetComponent<RigidbodyMovement>();

            m_currentAnimationState = m_idleAnimationID;
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

            PlayAnimations();
        }

        private void FixedUpdate()
        {
            if (m_playerToChase == null)
                return;

            if (!m_isAttacking)
            {
                Vector2 directionToMove = (m_playerToChase.transform.position - transform.position).normalized;

                if (directionToMove.x < 0 && m_facingRight || directionToMove.x > 0 && !m_facingRight)
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                    m_facingRight = !m_facingRight;
                }

                m_movement.Move(directionToMove);
            }
        }

        private void PlayAnimations()
        {
            int stateToPlay = GetAnimationState();

            if (stateToPlay == m_currentAnimationState)
                return;

            m_animator.CrossFade(stateToPlay, 0);
            m_currentAnimationState = stateToPlay;
        }

        private int GetAnimationState()
        {
            if (m_isAttacking && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && m_currentAnimationState == m_attackAnimationID)
                m_isAttacking = false;

            if (m_isAttacking)
                return m_attackAnimationID;

            if (m_playerToChase != null)
                return m_walkAnimationID;

            return m_idleAnimationID;
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