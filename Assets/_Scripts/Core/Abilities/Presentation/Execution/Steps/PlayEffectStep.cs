using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class PlayEffectStep : IAbilityExecutionStep
    {
        [SerializeField] private GameObject m_effectPrefab;

        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            var entityPosition = context.World.GetEntityPosition(context.ExecutorId);
            entityPosition = context.World.ToTileCenter(entityPosition.ToVector2Int());
            GameObject.Instantiate(m_effectPrefab, entityPosition, Quaternion.identity);

            return Task.CompletedTask;
        }
    }
}
