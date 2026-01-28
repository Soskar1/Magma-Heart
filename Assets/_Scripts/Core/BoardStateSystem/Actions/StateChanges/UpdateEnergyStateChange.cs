using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record UpdateEnergyStateChange(int UnitId, int OldEnergyValue, int NewEnergyValue) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken token)
        {
            if (actualBoard.Board.TryGetUnit(UnitId, out EntityModel model))
                model.Energy.CurrentEnergy = NewEnergyValue;

            return Task.CompletedTask;
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(UnitId, out EntityModel model))
                model.Energy.CurrentEnergy = NewEnergyValue;
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(UnitId, out EntityModel model))
                model.Energy.CurrentEnergy = OldEnergyValue;
        }
    }
}
