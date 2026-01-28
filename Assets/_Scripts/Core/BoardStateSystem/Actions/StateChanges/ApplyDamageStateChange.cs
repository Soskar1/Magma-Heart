using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageStateChange(int TargetId, float Damage) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            if (actualBoard.Board.TryGetUnit(TargetId, out EntityModel target))
                target.Health.CurrentHealth -= Damage;

            return Task.CompletedTask;
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(TargetId, out EntityModel target))
            {
                target.Health.CurrentHealth -= Damage;

                if (target.Health.CurrentHealth <= 0)
                    target.IsDisabled = true;
            }
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(TargetId, out EntityModel target))
            {
                target.Health.CurrentHealth += Damage;

                if (target.Health.CurrentHealth > 0)
                    target.IsDisabled = false;
            }
        }
    }
}
