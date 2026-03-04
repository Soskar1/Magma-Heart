using MagmaHeart.Abilities;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class RestoreParameterHandler : IEffectHandler<RestoreParameterEffect>
    {
        public void Handle(IBoardGameWorld world, RestoreParameterEffect effect)
        {
            IParameter parameter = world.GetParameter(effect.ExecutorId, effect.Parameter);
            float newValue = parameter.CurrentValue + effect.Amount;

            world.SetParameter(effect.ExecutorId, effect.Parameter, newValue);
        }
    }
}
