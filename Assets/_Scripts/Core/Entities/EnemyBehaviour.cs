using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Field Of View")]
        [SerializeField] private float m_triggerAttackDistance;
        [SerializeField] private float m_searchRadius;
        [SerializeField] private int m_amountOfRaycasts;
        private FieldOfView m_fieldOfView;

        [Header("Animation Events")]
        [SerializeField] private string m_takeDamageAnimationName;
        [SerializeField] private string m_deathAnimationName;

        private Entity m_entity;
        private IAttacker m_entityAttack;
        private IMovable m_entityMovement;
        private IAnimatable m_entityAnimations;

        private Entity m_entityToChase;
        private Rigidbody2D m_rigidbody;

        private bool m_canMove = true;

        private void Awake()
        {
            m_entity = GetComponent<Entity>();
            m_entityAttack = m_entity as IAttacker;
            m_entityMovement = m_entity as IMovable;
            m_entityAnimations = m_entity as IAnimatable;

            if (m_entityAttack == null)
                Debug.LogError($"Entity {transform.name} does not support IAttacker interface");

            if (m_entityMovement == null)
                Debug.LogError($"Entity {transform.name} does not support IMovable interface");

            if (m_entityAnimations == null)
                Debug.LogError($"Entity {transform.name} does not support IAnimatable interface");

            m_rigidbody = GetComponent<Rigidbody2D>();
            m_entityToChase = null;
            m_fieldOfView = new FieldOfView(m_searchRadius, m_amountOfRaycasts, transform);
        }

        private void OnEnable()
        {
            m_entity.Health.OnTakeDamage += DisableMovement;
            
            m_entity.Health.OnDeath += DisableMovement;
            m_entity.Health.OnDeath += DisableMovement;

            m_entityAttack.OnAttackEnded += EnableMovement;
            m_entityAnimations.AnimationPlayer.OnAnimationEnded += ProcessAnimationOnEndEvents;
        }

        private void OnDisable()
        {
            m_entity.Health.OnTakeDamage -= DisableMovement;
            
            m_entity.Health.OnDeath -= DisableMovement;
            m_entity.Health.OnDeath -= DisableMovement;

            m_entityAttack.OnAttackEnded -= EnableMovement;
            m_entityAnimations.AnimationPlayer.OnAnimationEnded -= ProcessAnimationOnEndEvents;
        }

        private void Update()
        {
            if (m_entityToChase == null)
            {
                m_entityToChase = m_fieldOfView.FindPlayer();
                if (m_entityToChase == null)
                    return;
            }

            if (Vector2.Distance(transform.position, m_entityToChase.Position) < m_triggerAttackDistance)
            {
                m_entityAttack.Attack();
                m_entityMovement.SetMovementDirection(Vector2.zero);
                m_rigidbody.linearVelocity = Vector2.zero;
                m_canMove = false;
            }

            if (m_canMove)
            {
                Vector2 directionToMove = (m_entityToChase.Position - m_entity.Position).normalized;
                m_entityMovement.SetMovementDirection(directionToMove);
            }
        }

        private void EnableMovement() => m_canMove = true;
        private void DisableMovement() => m_canMove = false;
        private void DisableEntity()
        {
            m_entity.enabled = false;
            enabled = false;
        }

        private void ProcessAnimationOnEndEvents(string endedAnimationName)
        {
            if (endedAnimationName == m_takeDamageAnimationName)
                EnableMovement();
            else if (endedAnimationName == m_deathAnimationName)
                DisableEntity();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector2 raycast = Vector2.right;

            for (int i = 0; i < m_amountOfRaycasts; ++i)
            {
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + raycast * m_searchRadius);
                raycast = raycast.Rotate(360.0f /  m_amountOfRaycasts);
            }
        }
    }
}