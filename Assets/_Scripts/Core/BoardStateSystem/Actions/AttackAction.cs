using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackAction : CombatAction<AttackActionArgs>
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        public AttackAction(EntityModel actionPossessor) : base(actionPossessor) { }

        public override IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnitModel unit) => new List<AttackActionArgs>() { new AttackActionArgs((EntityModel)unit) };

        public override int GetEnergyCost(AttackActionArgs args, BoardState gameState) => ENERGY_COST;

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            return changes.Concat(new List<StateChange>
            {
                new ApplyDamageStateChange(ActionPossessor, args.Target, ATTACK_DAMAGE),
            });
        }

        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            bool result = base.CanExecute(args, gameState);
            if (!result)
                return result;

            PositionPropertySnapshot possessorPosition = gameState.GetProperty<PositionPropertySnapshot>(ActionPossessor);
            PositionPropertySnapshot targetPosition = gameState.GetProperty<PositionPropertySnapshot>(args.Target);

            if (possessorPosition.ManhattanDistance(targetPosition) > ATTACK_DISTANCE)
                return false;

            return true;
        }
    }
}