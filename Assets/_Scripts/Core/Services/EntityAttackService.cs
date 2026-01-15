using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Services
{
    public class EntityAttackService
    {
        private readonly SpawnService m_spawner;

        public EntityAttackService(SpawnService spawner) => m_spawner = spawner;

        public async Task AttackEntityAsync(CombatBoardState boardState, Entity attacker, Entity target, float damage, AttackType attackType, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            int targetX = target.Model.GetCurrentTilePosition().x;
            int attackerX = attacker.Model.GetCurrentTilePosition().x;
            attacker.Facing.TryUpdateFacing(targetX - attackerX);

            await attacker.Animation.PlayAttackAnimationAsync();

            // Play idle as next animation because ranged attacks have a delay while the projectile is in the air
            // AnimationPlayer restarts the attack animation, but the animation trigger is not set -> crash
            // I think, we need to fix this, because this solution is a bit of a hack
            attacker.Animation.PlayeIdleAsNextAnimation();

            EntityModel targetToHit = target.Model;
            if (attackType == AttackType.Ranged)
                targetToHit = await WaitForProjectileHit(attacker, target);

            if (targetToHit != null)
                await ApplyDamage(boardState, attacker, target, damage, cancellationToken);

            await attacker.Animation.WaitForAnimationEnd();
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

        private async Task ApplyDamage(CombatBoardState boardState, Entity attacker, Entity target, float damage, CancellationToken cancellationToken)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(attacker.Model, target.Model, damage);
            await boardState.ApplyStateChangesAsync(new List<StateChange>() { damageStateChange }, cancellationToken);
        }
    }
}
