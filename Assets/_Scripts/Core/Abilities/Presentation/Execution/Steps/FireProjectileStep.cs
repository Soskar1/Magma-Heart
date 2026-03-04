using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class FireProjectileStep : IAbilityExecutionStep
    {
        [SerializeField] private Projectile m_projectilePrefab;

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            context.World.TryGetEntity(context.ExecutorId, out Entity executor);
            context.World.TryGetEntity(context.Plan.Target.EntityId, out Entity target);

            if (executor == null)
            {
                Debug.LogError($"Failed to find executor entity with ID {context.ExecutorId}");
                return;
            }

            if (target == null)
            {
                Debug.LogError($"Failed to find target entity with ID {context.Plan.Target.EntityId}");
                return;
            }

            Projectile projectile = UnityEngine.Object.Instantiate(m_projectilePrefab, executor.transform.position, Quaternion.identity);
            projectile.Initialize(executor.Model);
            projectile.transform.position = executor.transform.position;

            Vector2 direction = target.transform.position - executor.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            await projectile.OnHit();
        }
    }
}
