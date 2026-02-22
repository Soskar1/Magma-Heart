using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using System.Linq;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public class MoveHandler : IEffectHandler<MoveEffect>
    {
        public void Handle(GameWorld world, MoveEffect effect)
        {
            world.TryGetEntity(effect.ExecutorId, out Entity executor);

            var path = effect.Path;
            executor.transform.position = path.Last();

            DungeonTile firstTile = world.GetTile(path.First());
            DungeonTile lastTile = world.GetTile(path.Last());

            world.CurrentRoom.RemoveUnit(firstTile.Position);
            world.CurrentRoom.AddUnit(lastTile.Position, executor.Model);
            world.CurrentRoom.ChangeNodeType(firstTile.Position, BoardNodeType.Walkable);
            world.CurrentRoom.ChangeNodeType(lastTile.Position, BoardNodeType.Obstacle);
        }
    }
}
