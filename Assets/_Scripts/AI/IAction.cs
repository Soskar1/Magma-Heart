using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.AI
{
    public interface IAction
    {
        public AIUnit ActionPossessor { get; }

        public bool CanSimulate(StateSnapshot state, AIUnit target);
        public StateSnapshot Simulate(StateSnapshot state, AIUnit target);
        public void Execute();
    }
}
