using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    public abstract class Action
    {
        public AIUnit ActionPossessor { get; }

        public Action(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        internal List<ActionArgs> GetSimulationArguments(StateSnapshot state, SimulatedBoard board)
        {
            List<ActionArgs> args = new List<ActionArgs>();

            foreach (AIUnit unit in state.GetAllUnits())
            {
                if (unit == ActionPossessor)
                    continue;

                if ((ActionPossessor.IsPlayer && !unit.IsPlayer) ||
                    (!ActionPossessor.IsPlayer && unit.IsPlayer))
                {
                    args.Add(CreateActionArgs(state, board, unit));
                }
            }

            return args;
        }

        public abstract ActionArgs CreateActionArgs(StateSnapshot state, SimulatedBoard board, AIUnit unit);
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
        public Action(AIUnit actionPossessor) : base(actionPossessor) { }

        public abstract bool CanSimulate(StateSnapshot state, SimulatedBoard board, T args);
        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => Simulate(state, board, (T)args);
        public virtual StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, T args) => base.Simulate(state, board, args);

        public abstract void Execute(T args);
    }
}
