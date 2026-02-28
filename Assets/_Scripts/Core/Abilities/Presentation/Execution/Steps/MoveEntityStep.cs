using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            MoveEffect moveEffect = context.Plan.Effects
                .OfType<MoveEffect>()
                .FirstOrDefault();

            List<Vector3> tiledPath = moveEffect.Path
                .Select(point => (Vector3)context.World.ToTileCenter(point.ToVector2Int()))
                .ToList();

            if (entity.Model.TilePosition == tiledPath.Last().ToVector3Int())
                return;

            entity.Facing.TryUpdateFacing(tiledPath.Last().x - entity.Model.TilePosition.x);

            await entity.TileBasedMovement.StartMovementAsync(tiledPath);
        }
    }
}
