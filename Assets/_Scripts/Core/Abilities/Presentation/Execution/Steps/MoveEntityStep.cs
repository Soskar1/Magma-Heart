using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class MoveEntityStep : IAbilityExecutionStep
    {
        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);

            if (entity == null)
            {
                Debug.LogWarning($"{nameof(MoveEntityStep)}: entity with id '{context.ExecutorId}' was not found!");
                return;
            }

            MoveEffect moveEffect = context.Plan.Effects
                .OfType<MoveEffect>()
                .FirstOrDefault();

            if (moveEffect == null)
                return;

            List<Vector3> tiledPath = moveEffect.Path
                .Select(point => (Vector3)context.World.ToTileCenter(point.ToVector2Int()))
                .ToList();

            if (entity.Model.TilePosition == tiledPath.Last().ToVector3Int())
                return;

            RemoveUnitFromBoard(context.World, moveEffect.Path.First());

            entity.Facing.TryUpdateFacing(tiledPath.Last().x - entity.Model.TilePosition.x);
            await entity.TileBasedMovement.StartMovementAsync(tiledPath);

            AddUnitToBoard(context.World, moveEffect.Path.Last(), entity.Model);
        }

        private void RemoveUnitFromBoard(GameWorld world, Vector3 start)
        {
            Vector2 startTile = world.WorldToTilePosition(start);

            bool isUnitRemoved = world.RemoveUnit(startTile);

            if (!isUnitRemoved)
            {
                Debug.LogError($"{nameof(MoveEntityStep)} failed to remove unit from the board at position {startTile}");
                return;
            }

            world.ChangeNodeType(startTile, BoardNodeType.Walkable);
        }

        private void AddUnitToBoard(GameWorld world, Vector3 end, EntityModel entity)
        {
            Vector2 endTile = world.WorldToTilePosition(end);
            world.AddUnit(endTile, entity);
            world.ChangeNodeType(endTile, BoardNodeType.Obstacle);
        }
    }
}
