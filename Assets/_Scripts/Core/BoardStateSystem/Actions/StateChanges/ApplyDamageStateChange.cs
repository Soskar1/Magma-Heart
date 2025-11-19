using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageStateChange(EntityModel Unit, float Damage) : MagmaHeartStateChange
    {
        public override void ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            Unit.Health.TakeDamage(Damage);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(Unit);
            simulation.UpdateProperty(Unit, new HealthPropertySnapshot(Damage, health.MaxHealth));

            if (Damage <= 0)
                simulation.UpdateProperty(Unit, new IsAlivePropertySnapshot(false));
        }
    }
}
