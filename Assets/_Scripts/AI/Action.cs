using MagmaHeart.AI.Boards;
using System.Linq;

namespace MagmaHeart.AI
{
    public abstract class Action
    {
        public AIUnit ActionPossessor { get; }

        public Action(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        public abstract bool CanSimulate(StateSnapshot state, SimulatedBoard board, AIUnit target);
        public virtual StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
        {
            StateSnapshot newState = state with
            {
                StateProperties = state.StateProperties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy()),
                CurrentSimulationDepth = state.CurrentSimulationDepth + 1
            };

            return newState;
        }

        // TODO: Add abstract arguments
        public abstract void Execute();
    }
}
