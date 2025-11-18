namespace MagmaHeart.AI.States
{
    public abstract class GameState
    {
        public IStateReadWrite StateReadWrite { get; init; }

        public GameState(StateReadWrite stateReadWrite)
        {
            StateReadWrite = stateReadWrite;
        }

        public abstract void ApplyStateChange(StateChange stateChange);
    }
}