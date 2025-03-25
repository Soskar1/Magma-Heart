using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RigidbodyMovement : MonoBehaviour
    {
        [SerializeField] private float m_maxSpeed;
        [SerializeField] private float m_acceleration;
        [SerializeField] private float m_decceleration;
        [SerializeField] private float m_velocityPower;
        private Rigidbody2D m_rigidbody;

        private const float EPSILON = 0.01f;

        private void Awake() => m_rigidbody = GetComponent<Rigidbody2D>();

        public void Move(Vector2 direction)
        {
            Vector2 targetVelocity = direction * m_maxSpeed;
            Vector2 velocityDifference = targetVelocity - m_rigidbody.linearVelocity;
            float accelerationRate = targetVelocity.magnitude > EPSILON ? m_acceleration : m_decceleration;

            float movementX = Mathf.Pow(Mathf.Abs(velocityDifference.x) * accelerationRate, m_velocityPower) * Mathf.Sign(velocityDifference.x);
            float movementY = Mathf.Pow(Mathf.Abs(velocityDifference.y) * accelerationRate, m_velocityPower) * Mathf.Sign(velocityDifference.y);

            m_rigidbody.AddForce(new Vector2(movementX, movementY));
        }
    }
}