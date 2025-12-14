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

        public abstract IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, ActionPayload payload, IEnumerable<AIUnitModel> targets);
    }

    public abstract class UnitAction<TPayload> : UnitAction
        where TPayload : ActionPayload
    {
        public abstract IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, TPayload payload, IEnumerable<AIUnitModel> targets);
        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, ActionPayload payload, IEnumerable<AIUnitModel> targets) => CreateSimulationArguments(state, executor, (TPayload)payload, targets);
    }

    public abstract class UnitAction<TArgs, TPayload> : UnitAction<TPayload>
        where TArgs : ActionArgs
        where TPayload : ActionPayload
    {
        public async Task ExecuteAsync(TArgs args, BoardState boardState, CancellationToken cancellationToken) => await ExecuteAsync((ActionArgs)args, boardState, cancellationToken);

        public abstract IEnumerable<StateChange> ProduceChanges(TArgs args, BoardState boardState);
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((TArgs)args, boardState);

        public abstract bool CanExecute(TArgs args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((TArgs)args, boardState);
    }
}
