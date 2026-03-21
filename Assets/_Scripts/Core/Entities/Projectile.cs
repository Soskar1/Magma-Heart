using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_lifeTimeInSeconds;
        [SerializeField] private ParticleSystem m_particles;
        private float m_lifeTimeTimer;

        private EntityModel m_attacker;

        private TaskCompletionSource<EntityModel> m_projectileHit;

        public void Initialize(EntityModel attacker)
        {
            m_attacker = attacker;

            m_projectileHit = new TaskCompletionSource<EntityModel>();
            m_lifeTimeTimer = m_lifeTimeInSeconds;
        }

        public void Update()
        {
            transform.Translate(Vector2.right * m_speed * Time.deltaTime);

            if (m_lifeTimeTimer > 0)
            {
                m_lifeTimeTimer -= Time.deltaTime;
            }
            else
            {
                if (m_projectileHit != null)
                {
                    Debug.LogWarning($"{nameof(m_projectileHit)} is null");
                    m_projectileHit.SetResult(null);
                }

                DestroyProjectile();
            }
        }

        public Task<EntityModel> OnHit() => m_projectileHit.Task;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Entity entity))
            {
                if (entity.Model == m_attacker)
                    return;

                if (entity.Model.IsPlayer == m_attacker.IsPlayer ||
                    !entity.Model.IsPlayer == !m_attacker.IsPlayer)
                    return;

                m_projectileHit.TrySetResult(entity.Model);
            }
            else
            {
                m_projectileHit.TrySetResult(null);
            }

            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            m_particles.transform.parent = null;
            m_particles.Stop();

            Destroy(m_particles.gameObject, 2f);
            Destroy(gameObject);
        }
    }
}