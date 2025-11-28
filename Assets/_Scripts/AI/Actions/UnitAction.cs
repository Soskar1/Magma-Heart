using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.Actions
{
    public abstract class UnitAction
    {
        public AIUnitModel ActionPossessor { get; }

        public UnitAction(AIUnitModel actionPossessor) => ActionPossessor = actionPossessor;

        public async Task ExecuteAsync(ActionArgs args, BoardState boardState, CancellationToken cancellationToken)
        {
            IEnumerable<StateChange> changes = ProduceChanges(args, boardState);
            await boardState.ApplyStateChangesAsync(changes, cancellationToken);
        }

        internal void Execute(ActionArgs args, SimulatedBoardState boardState)
        {
            IEnumerable<StateChange> changes = ProduceChanges(args, boardState);
            boardState.ApplyStateChanges(changes);
        }

        public abstract IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState);
        public abstract bool CanExecute(ActionArgs args, BoardState boardState);
        internal List<ActionArgs> GetArguments(SimulatedBoardState state) // Right now it is a dumb solution
        {
            List<ActionArgs> args = new List<ActionArgs>();

            foreach (AIUnitModel unit in state.Board.GetUnits())
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
        
        public abstract IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnitModel unit);
    }

    public abstract class UnitAction<T> : UnitAction where T : ActionArgs
    {
        public UnitAction(AIUnitModel actionPossessor) : base(actionPossessor) { }

        public async Task ExecuteAsync(T args, BoardState boardState, CancellationToken cancellationToken) => await ExecuteAsync((ActionArgs)args, boardState, cancellationToken);

        public abstract IEnumerable<StateChange> ProduceChanges(T args, BoardState boardState);
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((T)args, boardState);

        public abstract bool CanExecute(T args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((T)args, boardState);
    }
}
