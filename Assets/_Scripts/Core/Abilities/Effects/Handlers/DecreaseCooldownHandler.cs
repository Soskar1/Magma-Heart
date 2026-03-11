using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class DecreaseCooldownHandler : IEffectHandler<DecreaseCooldownEffect>
    {
        public void Handle(IBoardGameWorld world, DecreaseCooldownEffect effect)
        {
            AIUnitModel unit = world.GetUnit(effect.ExecutorId);
        
            foreach (string cooldownId in unit.GetCooldownIds())
            {
                int currentCooldown = unit.GetCooldown(cooldownId);
                if (currentCooldown > 0)
                    unit.SetCooldown(cooldownId, currentCooldown - 1);
            }
        }
    }
}
