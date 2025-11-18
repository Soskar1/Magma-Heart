using MagmaHeart.AI.Boards;

namespace MagmaHeart.AI.States
{
    public class SimulatedBoardState : BoardState
    {
        private StateSnapshot m_stateProperties;
        public SimulatedBoardState(Board board) : base(board.DeepCopy())
        {
            m_stateProperties = StateSnapshotMaker.CreateStateSnapshot(Board.GetUnits());
        }

        public void Undo()
        {

        }

        public override T GetProperty<T>(AIUnit unit) => m_stateProperties.GetProperty<T>(unit);
        public void WriteProperty<T>(AIUnit unit, T property) where T : PropertySnapshot => m_stateProperties.Update(unit, property);
    }
}