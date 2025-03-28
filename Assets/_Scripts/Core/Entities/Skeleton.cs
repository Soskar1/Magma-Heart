using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Skeleton : MonoBehaviour, IHittable
    {
        [SerializeField] private float m_maxHealth;
        private Health m_health;

        private void Awake() => m_health = new Health(m_maxHealth);

        public void Hit(in float damage) => m_health.TakeDamage(damage);
    }
}