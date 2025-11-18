namespace MagmaHeart.AI.States
{
    public class SimulationReadWrite : IStateReadWrite
    {
        private Dictionary<AIUnit, TypeMap<PropertySnapshot>> m_stateProperties;
        public Board Board { get; init; }

        public SimulationReadWrite(Board board)
        {
            Board = board.DeepCopy();
            m_stateProperties = StateSnapshotMaker.CreateStateSnapshot(Board.Units.Values);
        }

        public T GetProperty<T>(AIUnit unit) where T : PropertySnapshot
        {
            return m_stateProperties[unit][typeof(T)]
        }

        public void Write<T>(T property) where T : PropertySnapshot
        {
            m_stateProperties[unit][typeof(T)] = property;
        }
    }
}