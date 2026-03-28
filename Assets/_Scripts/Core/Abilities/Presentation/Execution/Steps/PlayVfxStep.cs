using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class PlayVfxStep : IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);

            if (entity == null)
            {
                Debug.LogWarning($"{nameof(PlayVfxStep)}: entity with id '{context.ExecutorId}' was not found!");
                return Task.CompletedTask;
            }

            entity.VFXPresenter.Play();
            return Task.CompletedTask;
        }
    }
}
