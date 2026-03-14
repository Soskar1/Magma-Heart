using MagmaHeart.AI;
using MagmaHeart.AI.Boards;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class KnockbackHandler : IEffectHandler<KnockbackEffect>
    {
        public void Handle(IBoardGameWorld world, KnockbackEffect effect)
        {
            AIUnitModel model = world.GetUnit(effect.TargetId);
            var currentPosition = world.GetEntityPosition(model.Id);

            world.RemoveUnit(currentPosition);
            world.AddUnit(effect.NewPosition, model);
            world.ChangeNodeType(currentPosition, BoardNodeType.Walkable);
            world.ChangeNodeType(effect.NewPosition, BoardNodeType.Obstacle);
        }
    }
}
