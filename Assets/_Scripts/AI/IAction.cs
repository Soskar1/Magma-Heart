using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.AI
{
    public interface IAction
    {
        public StateSnapshot Simulate(StateSnapshot state);
        public void Execute();
    }
}
