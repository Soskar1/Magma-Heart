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
            List<StateChange> changes = ProduceChanges(args, boardState);
            boardState.ApplyStateChanges(changes);
        }

        public abstract List<StateChange> ProduceChanges(ActionArgs args, BoardState boardState);
        public abstract bool CanExecute(ActionArgs args, BoardState boardState);
        internal List<ActionArgs> GetArguments(BoardState state) // Right now it is a dumb solution
        {
            List<ActionArgs> args = new List<ActionArgs>();

            foreach (AIUnit unit in state.Board.GetUnits())
            {
                if (unit == ActionPossessor)
                    continue;

                if ((ActionPossessor.IsPlayer && !unit.IsPlayer) ||
                    (!ActionPossessor.IsPlayer && unit.IsPlayer))
                {
                    args.Add(CreateArgument(state, unit));
                }
            }

            return args;
        }
        
        public abstract ActionArgs CreateArgument(BoardState state, AIUnit unit);
    }

    public abstract class UnitAction<T> : UnitAction where T : ActionArgs
    {
        public UnitAction(AIUnit actionPossessor) : base(actionPossessor) { }

        public void Execute(T args, BoardState boardState) => Execute((ActionArgs)args, boardState);

        public abstract List<StateChange> ProduceChanges(T args, BoardState boardState);
        public override List<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((T)args, boardState);

        public abstract bool CanExecute(T args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((T)args, boardState);
    }
}
