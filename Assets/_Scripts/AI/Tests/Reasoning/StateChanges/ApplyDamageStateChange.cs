using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
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
                simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(false));

            targetHealth = new Health(targetHealth.CurrentHealth - Damage, targetHealth.MaxHealth);
            simulation.UpdateProperty(Target, targetHealth);
        }

        //public override void UndoChangeInSimulation(SimulatedBoardState simulation)
        //{
        //    Health targetHealth = simulation.GetProperty<Health>(Target);
        //    IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(Target);

        //    float health = targetHealth.CurrentHealth + Damage;

        //    if (!isAlive && health > 0)
        //        simulation.UpdateProperty(Target, new IsAlivePropertySnapshot(true));

        //    targetHealth = new Health(health, targetHealth.MaxHealth);
        //    simulation.UpdateProperty(Target, targetHealth);
        //}
    }
}
