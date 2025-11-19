using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    public abstract class UnitAction
    {
        public AIUnit ActionPossessor { get; }

        public UnitAction(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        public void Execute(ActionArgs args, BoardState boardState)
        {
            IEnumerable<StateChange> changes = ProduceChanges(args, boardState);
            boardState.ApplyStateChanges(changes);
        }

        public abstract IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState);
        public abstract bool CanExecute(ActionArgs args, BoardState boardState);
        internal List<ActionArgs> GetArguments(SimulatedBoardState state) // Right now it is a dumb solution
        {
            List<ActionArgs> args = new List<ActionArgs>();

            foreach (AIUnit unit in state.Board.GetUnits())
            {
                if (unit == ActionPossessor)
                    continue;

                if ((ActionPossessor.IsPlayer && !unit.IsPlayer) ||
                    (!ActionPossessor.IsPlayer && unit.IsPlayer))
                {
                    args.AddRange(CreateSimulationArgument(state, unit));
                }
            }

            return args;
        }
        
        public abstract IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnit unit);
    }

    public abstract class UnitAction<T> : UnitAction where T : ActionArgs
    {
        public UnitAction(AIUnit actionPossessor) : base(actionPossessor) { }

        public void Execute(T args, BoardState boardState) => Execute((ActionArgs)args, boardState);

        public abstract IEnumerable<StateChange> ProduceChanges(T args, BoardState boardState);
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((T)args, boardState);

        public abstract bool CanExecute(T args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((T)args, boardState);
    }
}
