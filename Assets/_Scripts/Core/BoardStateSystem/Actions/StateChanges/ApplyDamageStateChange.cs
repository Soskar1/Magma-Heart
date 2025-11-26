using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Entities.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageStateChange(EntityModel Attacker, EntityModel Target, float Damage) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            actualBoard.Room.TryGetEntityPresenter(Attacker, out EntityPresenter attackerEntity);
            actualBoard.Room.TryGetEntityPresenter(Target, out EntityPresenter targetEntity);

            return actualBoard.AttackService.AttackEntityAsync(attackerEntity, targetEntity, Damage, cancellationToken);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(Target);
            simulation.UpdateProperty(Target, new HealthPropertySnapshot(health.CurrentHealth - Damage, health.MaxHealth));

            if (Damage <= 0)
                simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(false));
        }
    }
}
