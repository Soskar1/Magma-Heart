using MagmaHeart.Abilities;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class DamageHandler : IEffectHandler<DamageEffect>
    {
        public void Handle(IBoardGameWorld world, DamageEffect effect)
        {
            IParameter parameter = world.GetParameter(effect.TargetId, effect.HealthId);
            float valueToSet = parameter.CurrentValue - effect.Damage;

            world.SetParameter(effect.TargetId, effect.HealthId, valueToSet);
        }
    }
}
