using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageStateChange(EntityModel Attacker, EntityModel Target, float Damage) : MagmaHeartStateChange
    {
        public override void ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            actualBoard.AttackService.AttackEntity(Attacker.Entity, Target.Entity, Damage);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(Target);
            simulation.UpdateProperty(Target, new HealthPropertySnapshot(Damage, health.MaxHealth));

            if (Damage <= 0)
                simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(false));
        }
    }
}
