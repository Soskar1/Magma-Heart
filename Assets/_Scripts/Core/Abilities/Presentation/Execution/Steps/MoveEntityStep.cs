using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System;
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

            Vector3 lastPathPosition = moveEffect.Path.Last();
            Vector3Int lastTilePosition = context.World.WorldToTilePosition(lastPathPosition).ToVector3Int();

            if (entity.Model.TilePosition == lastTilePosition)
                return;

            entity.Facing.TryUpdateFacing(lastPathPosition.x - entity.Model.TilePosition.x);

            await entity.TileBasedMovement.StartMovementAsync(moveEffect.Path);
        }
    }
}
