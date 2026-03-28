using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MagmaHeart.Core.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_lifeTimeInSeconds;
        [SerializeField] private ParticleSystem m_particles;
        [SerializeField] private Light2D m_light2D;
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Collider2D m_collider2D;
        private float m_lifeTimeTimer;
        private bool m_isDestroyed;

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
                    m_projectileHit.SetResult(null);

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
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            if (m_isDestroyed)
                return;

            m_isDestroyed = true;

            m_particles.Stop();
            m_renderer.enabled = false;
            m_light2D.enabled = false;
            m_collider2D.enabled = false;
            m_projectileHit = null;

            Destroy(m_particles.gameObject, 2f);
            Destroy(gameObject, 5f);

        }
    }
}