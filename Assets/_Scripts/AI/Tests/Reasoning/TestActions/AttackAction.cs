using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs, AttackActionPayload>
    {
        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            return new List<StateChange> {
                new ApplyDamageStateChange(args.Damage, args.Target)
            };
        }

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, AttackActionPayload payload, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
                yield return new AttackActionArgs(executor, unit, payload.Damage);
        }
    }
}
