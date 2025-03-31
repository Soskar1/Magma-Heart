using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Knockback : MonoBehaviour
    {
        private Rigidbody2D m_rigidbody;

        private void Awake() => m_rigidbody = GetComponent<Rigidbody2D>();

        public void ApplyKnockback(Vector2 direction, float strength) => m_rigidbody.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
    }
}