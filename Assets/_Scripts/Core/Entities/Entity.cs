using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private float m_maxHealth;
        private Health m_health;

        public Health Health => m_health;

        public IMovable Movement { get; private set; }
        public IMeleeAttacker MeleeAttack { get; private set; }
        public AnimationPlayer Animation { get; private set; }

        public void Initialize()
        {
            m_health = new Health(m_maxHealth);
            Movement = GetComponent<IMovable>();
            MeleeAttack = GetComponent<IMeleeAttacker>();
            Animation = GetComponent<AnimationPlayer>();

            if (Movement == null)
                Debug.LogError($"{name} is missing IMovable.");

            if (MeleeAttack == null)
                Debug.LogError($"{name} is missing IMeleeAttacker.");

            if (Animation == null)
                Debug.LogWarning($"{name} has no IAnimatable (optional).");

            Animation.Initialize();
        }

        public void Enable() => Animation.Enable();
        public void Disable() => Animation.Disable();
        public void Hit(in float damage) => m_health.TakeDamage(damage);
    }
}