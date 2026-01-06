using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.Args;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class CombatAction<TArgs, TInput, TData> : UnitAction<TArgs, TInput, TData>
        where TArgs : MagmaHeartActionArgs
        where TInput : MagmaHeartActionInput
        where TData : ActionData
    {
        public abstract int GetEnergyCost(TArgs args, BoardState boardState);

        public override bool CanExecute(TArgs args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(args.Input.Executor);

            if (energy.CurrentEnergy < GetEnergyCost(args, boardState))
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(TArgs args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(args.Input.Executor);

            return new List<StateChange>
            {
                new UpdateEnergyStateChange(args.TypedInput.TypedExecutor, energy.CurrentEnergy - GetEnergyCost(args, boardState))
            };
        }
    }
}
