using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField] private float m_damage;
        private IEntity m_owner;

        private void Awake() => m_owner = GetComponentInParent<IEntity>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out IEntity entity) && entity != m_owner)
                entity.Hit(m_damage);
        }
    }
}