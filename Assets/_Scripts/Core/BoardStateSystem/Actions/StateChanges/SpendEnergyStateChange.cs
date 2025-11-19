using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record SpendEnergyStateChange(EntityModel Unit, int EnergyToSpend) : MagmaHeartStateChange
    {
        public override void ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            Unit.Energy.Spend(EnergyToSpend);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(Unit);
            simulation.UpdateProperty(Unit, new EnergyPropertySnapshot(energy.CurrentEnergy - EnergyToSpend));
        }
    }
}
