using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private float m_maxHealth;
        private Health m_health;

        public Health Health => m_health;

        public virtual void Initialize() => m_health = new Health(m_maxHealth);

        public void Hit(in float damage) => m_health.TakeDamage(damage);
    }
}