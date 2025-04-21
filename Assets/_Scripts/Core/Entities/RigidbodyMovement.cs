using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Facing))]
    public class RigidbodyMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private float m_maxSpeed;
        [SerializeField] private float m_acceleration;
        [SerializeField] private float m_decceleration;
        [SerializeField] private float m_velocityPower;
        private Rigidbody2D m_rigidbody;
        private Facing m_facing;

        private const float EPSILON = 0.01f;

        private Vector2 m_currentMovementDirection;
        public Vector2 CurrentMovementDirection => m_currentMovementDirection;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_facing = GetComponent<Facing>();
        }

        public void Move(Vector2 direction)
        {
            m_facing.TryUpdateFacing(direction.x);
            m_currentMovementDirection = direction;

            Vector2 targetVelocity = direction * m_maxSpeed;
            Vector2 velocityDifference = targetVelocity - m_rigidbody.linearVelocity;
            float accelerationRate = targetVelocity.magnitude > EPSILON ? m_acceleration : m_decceleration;

            float movementX = Mathf.Pow(Mathf.Abs(velocityDifference.x) * accelerationRate, m_velocityPower) * Mathf.Sign(velocityDifference.x);
            float movementY = Mathf.Pow(Mathf.Abs(velocityDifference.y) * accelerationRate, m_velocityPower) * Mathf.Sign(velocityDifference.y);

            Vector2 movement = new Vector2(movementX, movementY);
            m_rigidbody.AddForce(movement);
            
        }

        public void IncreaseMaxSpeed(float amount) => m_maxSpeed += amount;
    }
}