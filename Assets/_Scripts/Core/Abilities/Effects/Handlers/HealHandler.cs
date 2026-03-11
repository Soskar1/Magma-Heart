using MagmaHeart.Abilities;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class HealHandler : IEffectHandler<HealEffect>
    {
        public void Handle(IBoardGameWorld world, HealEffect effect)
        {
            IParameter parameter = world.GetParameter(effect.ExecutorId, effect.HealthId);
            float valueToSet = parameter.CurrentValue + effect.HealAmount;

            world.SetParameter(effect.ExecutorId, effect.HealthId, valueToSet);
        }
    }
}
