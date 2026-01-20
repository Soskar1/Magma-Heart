using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageStateChange(EntityModel Attacker, EntityModel Target, float Damage) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            Target.Health.CurrentHealth -= Damage;
            return Task.CompletedTask;
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(Target);
            float newHealth = health.CurrentHealth - Damage;
            simulation.UpdateProperty(Target, new HealthPropertySnapshot(newHealth, health.MaxHealth));

            if (newHealth <= 0)
                simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(false));
        }
    }
}
