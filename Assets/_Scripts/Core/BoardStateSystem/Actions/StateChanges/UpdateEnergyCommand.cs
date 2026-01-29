using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record UpdateEnergyCommand(int ExecutorId, int NewEnergyValue) : IBoardCommand
    {
        private int m_oldEnergyValue;

        public void Execute(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel model);
            m_oldEnergyValue = model.Energy.CurrentEnergy;
            model.Energy.CurrentEnergy = NewEnergyValue;
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel model);
            model.Energy.CurrentEnergy = m_oldEnergyValue;
        }
    }
}
