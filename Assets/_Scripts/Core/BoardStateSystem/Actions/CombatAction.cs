using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Args;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class CombatAction<TArgs, TInput, TData> : UnitAction<TArgs, TInput, TData>
        where TArgs : MagmaHeartActionArgs
        where TInput : MagmaHeartActionInput
        where TData : ActionData
    {
        public abstract int GetEnergyCost(TArgs args, Board board);

        public override bool CanExecute(TArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out EntityModel model);

            if (model.Energy.CurrentEnergy < GetEnergyCost(args, board))
                return false;

            return true;
        }

        public override IEnumerable<IBoardCommand> Execute(TArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out EntityModel model);

            return new List<IBoardCommand>
            {
                new UpdateEnergyCommand(args.TypedInput.TypedExecutor.Id, model.Energy.CurrentEnergy - GetEnergyCost(args, board))
            };
        }
    }
}
