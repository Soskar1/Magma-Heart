using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Services
{
    public class EntityAttackService
    {
        public async void AttackEntity(Entity attacker, Entity target, float damage)
        {
            int targetX = target.Model.GetCurrentTilePosition().x;
            int attackerX = attacker.Model.GetCurrentTilePosition().x;
            attacker.Facing.TryUpdateFacing(targetX - attackerX);

            await attacker.Animation.PlayAttackAnimationAsync();
            
            target.Health.TakeDamage(damage);
        }
    }
}
