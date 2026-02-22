using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class DamageHandler : IEffectHandler<DamageEffect>
    {
        public void Handle(GameWorld world, DamageEffect effect)
        {
            world.TryGetEntity(effect.TargetId, out Entity target);
            target.Health.CurrentHealth -= effect.Damage;
        }
    }
}
