using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [System.Serializable]
    public class PlaySfxStep : IAbilityExecutionStep
    {
        [SerializeField] private List<AudioClip> m_sfx;

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

            var clip = m_sfx[Random.Range(0, m_sfx.Count)];

            entity.SfxPresenter.PlaySound(clip);
            return Task.CompletedTask;
        }
    }
}
