using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record AttackStateChange(EntityModel Attacker, EntityModel Target, float Damage, AttackType AttackType) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            actualBoard.Room.TryGetEntity(Attacker, out Entity attackerEntity);
            actualBoard.Room.TryGetEntity(Target, out Entity targetEntity);

            return actualBoard.Services.AttackService.AttackEntityAsync(actualBoard, attackerEntity, targetEntity, Damage, AttackType, cancellationToken);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(Attacker, Target, Damage);
            simulation.ProduceStateChange(damageStateChange);
        }
    }
}
