using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_triggerAttackDistance;
        [SerializeField] private float m_searchRadius;
        [SerializeField] private int m_amountOfRaycasts;
        private FieldOfView m_fieldOfView;

        private IEntity m_entityToControl;
        private IEntity m_entityToChase;
        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_entityToControl = GetComponent<IEntity>();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_entityToChase = null;
            m_fieldOfView = new FieldOfView(m_searchRadius, m_amountOfRaycasts, transform);
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
                m_entityToControl.Attack();
                m_entityToControl.ApplyMovement(Vector2.zero);
                m_rigidbody.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 directionToMove = (m_entityToChase.Position - m_entityToControl.Position).normalized;
                m_entityToControl.ApplyMovement(directionToMove);
            }
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