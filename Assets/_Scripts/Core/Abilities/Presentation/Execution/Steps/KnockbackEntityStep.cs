using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class KnockbackEntityStep : IAbilityExecutionStep
    {
        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            KnockbackEffect knockbackEffect = context.Plan.Effects
                .OfType<KnockbackEffect>()
                .FirstOrDefault();

            if (knockbackEffect == null)
                return;

            if (context.World.GetNodeType(knockbackEffect.NewPosition) == BoardNodeType.Obstacle)
            {
                Debug.LogWarning($"{nameof(KnockbackEntityStep)}: cannot knockback entity to position {knockbackEffect.NewPosition} because it is an obstacle!");
                return;
            }

            context.World.TryGetEntity(knockbackEffect.TargetId, out Entity entity);

            if (entity == null)
                return;

            Vector3 targetPosition = context.World.GetEntityPosition(knockbackEffect.TargetId);

            RemoveUnitFromBoard(context.World, targetPosition);

            var start = context.World.WorldGrid.ToTileCenter(targetPosition.ToVector2Int());
            var end = context.World.WorldGrid.ToTileCenter(knockbackEffect.NewPosition.ToVector2Int());
            entity.TileBasedMovement.StartMovementAsync(new List<Vector3>() { start, end });

            AddUnitToBoard(context.World, knockbackEffect.NewPosition, entity.Model);
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
