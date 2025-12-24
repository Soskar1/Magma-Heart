using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Spawning;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            if (attackType == AttackType.Melee)
                await ApplyDamage(attacker, target, damage, cancellationToken);
            else
                await WaitForProjectileHit(attacker, target, damage);
        }

        private async Task ApplyDamage(Entity attacker, Entity target, float damage, CancellationToken cancellationToken)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(attacker.Model, target.Model, damage);
            await m_boardState.ApplyStateChangesAsync(new List<StateChange>() { damageStateChange }, cancellationToken);
        }

        private async Task WaitForProjectileHit(Entity attacker, Entity target, float damage)
        {
            Projectile projectile = m_spawner.ProjectileSpawner.Spawn(attacker.transform.position, attacker.Model, target.Model, damage, m_boardState);
            await projectile.OnHit();
        }
    }
}
