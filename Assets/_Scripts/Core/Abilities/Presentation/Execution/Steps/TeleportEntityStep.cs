using System;
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
    public class TeleportEntityStep : IAbilityExecutionStep
    {
        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);

            if (entity == null)
            {
                Debug.LogWarning($"{nameof(TeleportEntityStep)}: entity with id '{context.ExecutorId}' was not found!");
                return;
            }

            TeleportEffect teleportEffect = context.Plan.Effects
                .OfType<TeleportEffect>()
                .FirstOrDefault();

            if (teleportEffect == null)
                return;

            var start = context.World.GetEntityPosition(context.ExecutorId);
            RemoveUnitFromBoard(context.World, start);
            entity.transform.position = context.World.WorldGrid.ToTileCenter(teleportEffect.TeleportPosition.ToVector2Int());
            AddUnitToBoard(context.World, teleportEffect.TeleportPosition, entity.Model);
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
