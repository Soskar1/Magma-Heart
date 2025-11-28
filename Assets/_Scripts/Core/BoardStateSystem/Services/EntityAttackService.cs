using MagmaHeart.Core.Entities.Presenters;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Services
{
    public class EntityAttackService
    {
        public async Task AttackEntityAsync(EntityPresenter attacker, EntityPresenter target, float damage, CancellationToken cancellationToken)
        {
            int targetX = target.Model.GetCurrentTilePosition().x;
            int attackerX = attacker.Model.GetCurrentTilePosition().x;
            attacker.Facing.TryUpdateFacing(targetX - attackerX);

            await attacker.Animation.PlayAttackAnimationAsync();
            
            target.Health.CurrentHealth -= damage;

            if (cancellationToken.IsCancellationRequested)
                return;

            await attacker.Animation.WaitForAnimationEnd();
            attacker.Animation.PlayIdleAnimation();
        }
    }
}
