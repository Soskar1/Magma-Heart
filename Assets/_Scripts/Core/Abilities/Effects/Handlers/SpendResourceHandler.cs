using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public sealed class SpendResourceHandler : IEffectHandler<SpendResourceEffect>
    {
        public void Handle(GameWorld world, SpendResourceEffect effect)
        {
            world.TryGetEntity(effect.ExecutorId, out Entity executor);

            // TODO: Prototype. This should be more generic and not hardcoded to energy.
            if (effect.Resource.Id == executor.Model.Energy.ResourceId.Id)
            {
                executor.Model.Energy.CurrentEnergy -= effect.Amount;
            }
        }
    }
}
