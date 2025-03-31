using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField] private float m_damage;
        [SerializeField] private float m_knockbackForce;
        private Entity m_owner;

        private void Awake() => m_owner = GetComponentInParent<Entity>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Entity entity) && entity != m_owner)
            {
                entity.Hit(m_damage);

                if (entity.TryGetComponent(out Knockback knockback))
                {
                    Vector2 knockbackDirection = entity.transform.position - m_owner.transform.position;
                    knockback.ApplyKnockback(knockbackDirection, m_knockbackForce);
                }
            }
        }
    }
}