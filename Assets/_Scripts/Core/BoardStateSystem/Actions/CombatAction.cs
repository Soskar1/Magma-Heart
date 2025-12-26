using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class CombatAction : UnitAction
    {
        public abstract int GetEnergyCost(ActionArgs args, BoardState boardState);

        public override bool CanExecute(ActionArgs args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(args.Executor);

            if (energy.CurrentEnergy < GetEnergyCost(args, boardState))
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(args.Executor);

            return new List<StateChange>
            {
                new UpdateEnergyStateChange((EntityModel)args.Executor, energy.CurrentEnergy - GetEnergyCost(args, boardState))
            };
        }
    }
}
