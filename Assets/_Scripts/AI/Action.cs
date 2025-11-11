using MagmaHeart.AI.Boards;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI
{
    public abstract class Action
    {
        public AIUnit ActionPossessor { get; }

        public Action(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        public abstract bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args);
        public virtual StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, ActionArgs args)
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

        public abstract void Execute(ActionArgs args);
    }

    public abstract class Action<T> : Action where T : ActionArgs
    {
        protected Action(AIUnit actionPossessor) : base(actionPossessor) { }

        public List<T> SimulationArgs { get; init; }

        public abstract bool CanSimulate(StateSnapshot state, SimulatedBoard board, T args);
        public virtual StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, T args) => base.Simulate(state, board, args);

        public abstract void Execute(T args);
    }
}
