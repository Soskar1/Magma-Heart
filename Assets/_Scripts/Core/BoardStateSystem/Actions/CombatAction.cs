using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class CombatAction<T> : UnitAction<T> where T : ActionArgs<EntityModel>
    {
        public abstract int GetEnergyCost(T args, BoardState boardState);

        public override bool CanExecute(T args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(args.Executor);

            if (energy.CurrentEnergy < GetEnergyCost(args, boardState))
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(T args, BoardState boardState)
        {
            return new List<StateChange>
            {
                new UpdateEnergyStateChange(args.TypedExecutor, args.TypedExecutor.Energy.CurrentEnergy - GetEnergyCost(args, boardState))
            };
        }
    }
}
