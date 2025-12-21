using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Services
{
    public class EntityAttackService
    {
        private readonly CombatBoardState m_boardState;

        public EntityAttackService(CombatBoardState boardState) => m_boardState = boardState;

        public async Task AttackEntityAsync(Entity attacker, Entity target, float damage, AttackType attackType, CancellationToken cancellationToken)
        {
            int targetX = target.Model.GetCurrentTilePosition().x;
            int attackerX = attacker.Model.GetCurrentTilePosition().x;
            attacker.Facing.TryUpdateFacing(targetX - attackerX);

            await attacker.Animation.PlayAttackAnimationAsync();
            
            if (attackType == AttackType.Melee)
                await ApplyDamage(attacker, target, damage, cancellationToken);
            else
                Debug.Log("[EntityAttackService] Handle ranged attacks!");

            if (cancellationToken.IsCancellationRequested)
                return;

            await attacker.Animation.WaitForAnimationEnd();
            attacker.Animation.PlayIdleAnimation();
        }

        private async Task ApplyDamage(Entity attacker, Entity target, float damage, CancellationToken cancellationToken)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(attacker.Model, target.Model, damage);
            await m_boardState.ApplyStateChangesAsync(new List<StateChange>() { damageStateChange }, cancellationToken);
        }
    }
}
