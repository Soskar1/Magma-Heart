using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class CombatAction<T> : UnitAction<T> where T : ActionArgs
    {
        public new EntityModel ActionPossessor { get; }

        public CombatAction(EntityModel actionPossessor) : base(actionPossessor)
        {
            ActionPossessor = actionPossessor;
        }

        public abstract int GetEnergyCost(T args, BoardState boardState);

        public override bool CanExecute(T args, BoardState boardState)
        {
            EnergyPropertySnapshot energy = boardState.GetProperty<EnergyPropertySnapshot>(ActionPossessor);

            if (energy.CurrentEnergy < GetEnergyCost(args, boardState))
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(T args, BoardState boardState)
        {
            return new List<StateChange>
            {
                new SpendEnergyStateChange(ActionPossessor, GetEnergyCost(args, boardState))
            };
        }
    }
}
