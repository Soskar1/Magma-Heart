using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Entity
    {
        private AnimationPlayer m_animation;

        public Transform Transform { get; private set; }
        public Health Health { get; private set; }
        public Energy Energy { get; private set; }
        public EntityData Data { get; private set; }
        public EntityStats Stats => Data.Stats;

        public Entity(EntityData data, Transform transform, AnimationPlayer animationPlayer = null)
        {
            Data = data;
            Transform = transform;
            m_animation = animationPlayer;

            Health = new Health(Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
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
    }
}