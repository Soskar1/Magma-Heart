using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Spawning;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Services
{
    public class EntityAttackService
    {
        private readonly CombatBoardState m_boardState;
        private readonly MagmaHeartSpawner m_spawner;

        public EntityAttackService(CombatBoardState boardState, MagmaHeartSpawner spawner)
        {
            m_boardState = boardState;
            m_spawner = spawner;
        }

        public async Task AttackEntityAsync(Entity attacker, Entity target, float damage, AttackType attackType, CancellationToken cancellationToken)
        {
            int targetX = target.Model.GetCurrentTilePosition().x;
            int attackerX = attacker.Model.GetCurrentTilePosition().x;
            attacker.Facing.TryUpdateFacing(targetX - attackerX);

            await attacker.Animation.PlayAttackAnimationAsync();
            attacker.Animation.NextAnimationState = attacker.Animation.GetIdleAnimation();

            EntityModel targetToHit = target.Model;
            if (attackType == AttackType.Ranged)
                targetToHit = await WaitForProjectileHit(attacker, target);

            if (targetToHit != null)
                await ApplyDamage(attacker, target, damage, cancellationToken);
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

        private async Task ApplyDamage(Entity attacker, Entity target, float damage, CancellationToken cancellationToken)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(attacker.Model, target.Model, damage);
            await m_boardState.ApplyStateChangesAsync(new List<StateChange>() { damageStateChange }, cancellationToken);
        }
    }
}
