using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackAction : CombatAction<AttackActionArgs>
    {
        public override int GetEnergyCost(AttackActionArgs args, BoardState gameState) => args.EnergyCost;

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            return changes.Concat(new List<StateChange>
            {
                new ApplyDamageStateChange(args.TypedExecutor, args.Target, args.AttackDamage),
            });
        }

        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            bool result = base.CanExecute(args, gameState);
            if (!result)
                return result;

            PositionPropertySnapshot possessorPosition = gameState.GetProperty<PositionPropertySnapshot>(args.TypedExecutor);
            PositionPropertySnapshot targetPosition = gameState.GetProperty<PositionPropertySnapshot>(args.Target);

            if (possessorPosition.ManhattanDistance(targetPosition) > args.AttackDistance)
                return false;

            return true;
        }
    }
}