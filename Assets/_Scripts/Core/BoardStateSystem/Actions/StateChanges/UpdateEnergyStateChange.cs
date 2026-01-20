using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record UpdateEnergyStateChange(EntityModel Unit, int NewEnergyValue) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken token)
        {
            Unit.Energy.CurrentEnergy = NewEnergyValue;
            return Task.CompletedTask;
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(Unit);
            simulation.UpdateProperty(Unit, new EnergyPropertySnapshot(NewEnergyValue, energy.MaxEnergy));
        }
    }
}
