using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.Actions
{
    public abstract class UnitAction
    {
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
        
        public abstract IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, IEnumerable<AIUnitModel> targets);
    }

    public abstract class UnitAction<T> : UnitAction where T : ActionArgs
    {
        public async Task ExecuteAsync(T args, BoardState boardState, CancellationToken cancellationToken) => await ExecuteAsync((ActionArgs)args, boardState, cancellationToken);

        public abstract IEnumerable<StateChange> ProduceChanges(T args, BoardState boardState);
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((T)args, boardState);

        public abstract bool CanExecute(T args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((T)args, boardState);
    }
}
