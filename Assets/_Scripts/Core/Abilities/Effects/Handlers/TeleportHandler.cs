using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class TeleportHandler : IEffectHandler<TeleportEffect>
    {
        public void Handle(IBoardGameWorld world, TeleportEffect effect)
        {
            AIUnitModel model = world.GetUnit(effect.ExecutorId);
            var start = world.GetEntityPosition(model.Id);

            var end = effect.TeleportPosition;
            world.MoveUnit(model.Id, end);

            Vector2 firstTile = world.WorldToTilePosition(start);
            Vector2 lastTile = world.WorldToTilePosition(end);

            world.RemoveUnit(firstTile);
            world.AddUnit(lastTile, model);
            world.ChangeNodeType(firstTile, BoardNodeType.Walkable);
            world.ChangeNodeType(lastTile, BoardNodeType.Obstacle);
        }
    }
}
