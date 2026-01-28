using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record AttackStateChange(int AttackerId, int TargetId, float Damage, AttackType AttackType) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            actualBoard.Room.TryGetEntity(AttackerId, out Entity attackerEntity);
            actualBoard.Room.TryGetEntity(TargetId, out Entity targetEntity);

            return actualBoard.Services.AttackService.AttackEntityAsync(actualBoard, attackerEntity, targetEntity, Damage, AttackType, cancellationToken);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(TargetId, Damage);
            simulation.ProduceStateChange(damageStateChange);
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(TargetId, Damage);
            simulation.UndoStateChange(damageStateChange);
        }
    }
}
