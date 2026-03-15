using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class AdjustFacingStep : IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            context.World.TryGetEntity(context.ExecutorId, out Entity executor);

            if (executor == null)
            {
                Debug.LogWarning($"{nameof(AdjustFacingStep)}: entity with id '{context.ExecutorId}' was not found!");
                return Task.CompletedTask;
            }

            DamageEffect damageEffect = context.Plan.Effects
                .OfType<DamageEffect>()
                .FirstOrDefault();

            if (damageEffect == null)
                return Task.CompletedTask;

            if (context.World.TryGetEntity(damageEffect.TargetId, out Entity targetEntity))
                executor.Facing.TryUpdateFacing(targetEntity.transform.position.x - executor.transform.position.x);

            return Task.CompletedTask;
        }
    }
}
