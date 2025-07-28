namespace MagmaHeart.Core.Entities
{
    public class Entity
    {
        private AnimationPlayer m_animation;

        public Health Health { get; private set; }

        public Entity(float health, AnimationPlayer animationPlayer = null)
        {
            Health = new Health(health);
            m_animation = animationPlayer;
        }

        public void Enable()
        {
            if (m_animation == null)
                return;

            m_animation.Enable();
        }

        public void Disable()
        {
            if (m_animation == null)
                return;

            m_animation.Disable();
        }

        public void Reset() => Health.Reset();

        public void RunAnimations()
        {
            if (m_animation == null)
                return;

            m_animation.PlayAnimations();
        }

        public void Hit(in float damage) => Health.TakeDamage(damage);
    }
}