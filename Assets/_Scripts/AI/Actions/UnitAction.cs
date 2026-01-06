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
        public abstract bool TryCreateArgs(ActionInput input, ActionData data, BoardState boardState, out ActionArgs args);
        public abstract bool TryGenerateArgs(AIUnitModel executor, ActionData data, BoardState boardState, out ActionArgs args);
    }

    public abstract class UnitAction<TArgs, TInput, TData> : UnitAction
        where TArgs : ActionArgs
        where TInput : ActionInput
        where TData : ActionData
    {
        public abstract IEnumerable<StateChange> ProduceChanges(TArgs args, BoardState boardState);
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => ProduceChanges((TArgs)args, boardState);

        public abstract bool CanExecute(TArgs args, BoardState boardState);
        public override bool CanExecute(ActionArgs args, BoardState boardState) => CanExecute((TArgs)args, boardState);

        public abstract bool TryCreateArgs(TInput input, TData data, BoardState boardState, out TArgs args);
        public override bool TryCreateArgs(ActionInput input, ActionData data, BoardState boardState, out ActionArgs args)
        {
            if (input is not TInput typedInput)
            {
                args = null;
                return false;
            }

            if (data is not TData typedData)
            {
                args = null;
                return false;
            }

            if (!TryCreateArgs(typedInput, typedData, boardState, out TArgs typedArgs))
            {
                args = null;
                return false;
            }

            args = typedArgs;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, ActionData data, BoardState boardState, out ActionArgs args)
        {
            args = null;

            if (data is not TData typedData)
                return false; 

            return TryGenerateArgs(executor, typedData, boardState, out args);
        }

        public abstract bool TryGenerateArgs(AIUnitModel executor, TData data, BoardState boardState, out ActionArgs args);
    }
}
