using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class MoveHandler : IEffectHandler<MoveEffect>
    {
        public void Handle(IBoardGameWorld world, MoveEffect effect)
        {
            AIUnitModel model = world.GetUnit(effect.ExecutorId);

            var path = effect.Path;
            world.MoveUnit(model.Id, path.Last());

            Vector2 firstTile = world.WorldToTilePosition(path.First());
            Vector2 lastTile = world.WorldToTilePosition(path.Last());

            world.RemoveUnit(firstTile);
            world.AddUnit(lastTile, model);
            world.ChangeNodeType(firstTile, BoardNodeType.Walkable);
            world.ChangeNodeType(lastTile, BoardNodeType.Obstacle);
        }
    }
}
