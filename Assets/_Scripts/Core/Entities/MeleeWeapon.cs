using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField] private float m_damage;
        private IHittable m_owner;

        private void Awake() => m_owner = GetComponentInParent<IHittable>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out IHittable entity) && entity != m_owner)
                entity.Hit(m_damage);
        }
    }
}