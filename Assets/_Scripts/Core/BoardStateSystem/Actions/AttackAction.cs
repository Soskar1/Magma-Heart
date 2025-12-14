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
    public class AttackAction : CombatAction<AttackActionArgs, AttackActionPayload>
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, AttackActionPayload payload, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
                yield return new AttackActionArgs((EntityModel)executor, payload, (EntityModel)unit); // TODO: Remove casts
        }

        public override int GetEnergyCost(AttackActionArgs args, BoardState gameState) => args.Payload.EnergyCost;

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            return changes.Concat(new List<StateChange>
            {
                new ApplyDamageStateChange(args.TypedExecutor, args.Target, args.Payload.AttackDamage),
            });
        }

        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            bool result = base.CanExecute(args, gameState);
            if (!result)
                return result;

            PositionPropertySnapshot possessorPosition = gameState.GetProperty<PositionPropertySnapshot>(args.TypedExecutor);
            PositionPropertySnapshot targetPosition = gameState.GetProperty<PositionPropertySnapshot>(args.Target);

            if (possessorPosition.ManhattanDistance(targetPosition) > args.Payload.AttackDistance)
                return false;

            return true;
        }
    }
}