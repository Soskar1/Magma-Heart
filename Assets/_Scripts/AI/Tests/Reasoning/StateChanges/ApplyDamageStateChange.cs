using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record ApplyDamageStateChange(float Damage, AIUnitModel Target) : StateChange
    {
        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            Health targetHealth = simulation.GetProperty<Health>(Target);

            if (targetHealth.CurrentHealth < Damage)
                simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(false));

            targetHealth = new Health(targetHealth.CurrentHealth - Damage, targetHealth.MaxHealth);
            simulation.UpdateProperty(Target, targetHealth);
        }
    }
}
