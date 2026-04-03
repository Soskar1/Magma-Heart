using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class StunHandler : IEffectHandler<StunEffect>
    {
        public void Handle(IBoardGameWorld world, StunEffect effect)
        {
            var targetUnit = world.GetUnit(effect.TargetId);
            targetUnit.SkipNextTurn();
        }
    }
}
