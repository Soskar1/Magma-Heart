using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    public abstract class UnitAction
    {
        public abstract IEnumerable<IBoardCommand> Execute(ActionArgs args, Board board);
        public abstract bool CanExecute(ActionArgs args, Board board);
        public abstract bool TryCreateArgs(ActionInput input, ActionData data, Board board, out ActionArgs args);
        public abstract bool TryGenerateArgs(AIUnitModel executor, ActionData data, Board board, out ActionArgs args);
    }

    public abstract class UnitAction<TArgs, TInput, TData> : UnitAction
        where TArgs : ActionArgs
        where TInput : ActionInput
        where TData : ActionData
    {
        public abstract IEnumerable<IBoardCommand> Execute(TArgs args, Board board);
        public override IEnumerable<IBoardCommand> Execute(ActionArgs args, Board board) => Execute((TArgs)args, board);

        public abstract bool CanExecute(TArgs args, Board board);
        public override bool CanExecute(ActionArgs args, Board board) => CanExecute((TArgs)args, board);

        public abstract bool TryCreateArgs(TInput input, TData data, Board board, out TArgs args);
        public override bool TryCreateArgs(ActionInput input, ActionData data, Board board, out ActionArgs args)
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

            if (!TryCreateArgs(typedInput, typedData, board, out TArgs typedArgs))
            {
                args = null;
                return false;
            }

            args = typedArgs;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, ActionData data, Board board, out ActionArgs args)
        {
            args = null;

            if (data is not TData typedData)
                return false; 

            return TryGenerateArgs(executor, typedData, board, out args);
        }

        public abstract bool TryGenerateArgs(AIUnitModel executor, TData data, Board board, out ActionArgs args);
    }
}
