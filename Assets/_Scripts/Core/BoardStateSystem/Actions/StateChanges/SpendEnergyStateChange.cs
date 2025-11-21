using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record SpendEnergyStateChange(EntityModel Unit, int EnergyToSpend) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            Unit.Energy.Spend(EnergyToSpend);
            return Task.CompletedTask;
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(Unit);
            simulation.UpdateProperty(Unit, new EnergyPropertySnapshot(energy.CurrentEnergy - EnergyToSpend));
        }
    }
}
