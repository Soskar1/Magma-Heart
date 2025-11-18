using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests.StateChanges
{
    internal record ApplyDamageStateChange(float Damage, AIUnit Target) : StateChange
    {
        public override void ApplyChangeToActualState(ActualBoardState actualBoard)
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            Health targetHealth = simulation.GetProperty<Health>(Target);

            if (targetHealth.CurrentHealth < Damage)
                simulation.WriteProperty(Target, new IsAlivePropertySnapshot(false));

            targetHealth = new Health(targetHealth.CurrentHealth - Damage, targetHealth.MaxHealth);
            simulation.WriteProperty(Target, targetHealth);
        }
    }
}
