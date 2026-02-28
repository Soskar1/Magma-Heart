using MagmaHeart.AI.Boards;
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
    public class OccupyBoardSpaceStep : IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            MoveEffect moveEffect = context.Plan.Effects.OfType<MoveEffect>().FirstOrDefault();

            if (moveEffect == null)
            {
                Debug.LogWarning($"{nameof(FreeBoardSpaceStep)} requires a MoveEffect in the plan to determine which space to free.");
                return Task.CompletedTask;
            }

            context.World.TryGetEntity(context.ExecutorId, out Entity executor);

            if (executor == null)
            {
                Debug.LogWarning($"{nameof(FreeBoardSpaceStep)}: entity with id '{context.ExecutorId}' was not found!");
                return Task.CompletedTask; 
            }

            Vector3 end = moveEffect.Path.Last();
            Vector2 endTile = context.World.WorldToTilePosition(end);

            context.World.CurrentRoom.AddUnit(endTile, executor.Model);
            context.World.CurrentRoom.ChangeNodeType(endTile, BoardNodeType.Obstacle);
            return Task.CompletedTask;
        }
    }
}
