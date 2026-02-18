using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Commands;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Services;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Presenters
{
    public class ApplyDamageCommandPresenter : IBoardCommandPresenter<ApplyDamageCommand>
    {
        private readonly CommandRunner m_commandRunner;
        private readonly SpawnService m_spawner;

        public ApplyDamageCommandPresenter(CommandRunner commandRunner, SpawnService spawner)
        {
            m_commandRunner = commandRunner;
            m_spawner = spawner;
        }

        public async Task Present(Room room, ApplyDamageCommand command, CancellationToken token)
        {
            // Attack animation
            room.TryGetEntity(command.ExecutorId, out Entity executor);
            room.TryGetEntity(command.TargetId, out Entity target);

            int animationHash = executor.Animation.PlayAttackAnimation();
            Task animationEndTask = executor.Animation.WaitForStateToFinish(animationHash);

            // Wait for event
            await executor.Animation.GetAnimationTriggerTask();

            if (command.AttackType == AttackType.Ranged)
                await WaitForProjectileHit(executor, target);

            m_commandRunner.Apply(room, command);

            // Wait for animation end
            if (!animationEndTask.IsCompleted)
                await animationEndTask;

            executor.Animation.PlayIdleAnimation();
        }

        private async Task<EntityModel> WaitForProjectileHit(Entity attacker, Entity target)
        {
            Projectile projectile = m_spawner.ProjectileSpawner.Spawn(attacker.Model);
            projectile.transform.position = attacker.transform.position;

            Vector2 direction = target.transform.position - attacker.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            return await projectile.OnHit();
        }
    }
}
