using MagmaHeart.AI;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class KillEveryoneHandler : IEffectHandler<KillEveryoneEffect>
    {
        public void Handle(IBoardGameWorld world, KillEveryoneEffect effect)
        {
            foreach (var entityId in effect.EntitiesToKill)
            {
                var unit = (EntityModel)world.GetUnit(entityId);
                unit.Health.CurrentHealth = 0;
            }
        }
    }
}
