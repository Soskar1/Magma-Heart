using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public sealed class SpendResourceHandler : IEffectHandler<SpendResourceEffect>
    {
        public void Handle(IBoardGameWorld world, SpendResourceEffect effect)
        {
            IParameter parameter = world.GetParameter(effect.ExecutorId, effect.Parameter);
            float newValue = parameter.CurrentValue - effect.Amount;

            world.SetParameter(effect.ExecutorId, effect.Parameter, newValue);
        }
    }
}
