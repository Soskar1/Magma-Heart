using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class FreeBoardSpaceStep : IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            MoveEffect moveEffect = context.Plan.Effects.OfType<MoveEffect>().FirstOrDefault();

            if (moveEffect == null)
            {
                Debug.LogError($"{nameof(FreeBoardSpaceStep)} requires a MoveEffect in the plan to determine which space to free.");
                return Task.CompletedTask;
            }

            Vector3 start = moveEffect.Path.First();
            Vector2 startTile = context.World.WorldToTilePosition(start).ToVector2();

            bool isUnitRemoved = context.World.CurrentRoom.RemoveUnit(startTile);

            if (!isUnitRemoved)
            {
                Debug.LogError($"{nameof(FreeBoardSpaceStep)} failed to remove unit from the board at position {startTile}");
                return Task.CompletedTask;
            }

            context.World.CurrentRoom.ChangeNodeType(startTile, BoardNodeType.Walkable);
            return Task.CompletedTask;
        }
    }
}
