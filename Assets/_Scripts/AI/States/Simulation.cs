namespace MagmaHeart.AI.States
{
    public class Simluation : GameState
    {
        public Simulation(Board board) : base(new SimulationReadWrite(board)) { }

        public override void ApplyStateChange(StateChange stateChange)
        {
            
        }

        public void Undo()
        {
            
        }
    }
}