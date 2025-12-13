using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs>
    {
        public float Damage { get; init; }

        public AttackAction(AIUnitModel actionPossessor, float damage) : base(actionPossessor)
        {
            Damage = damage;
        }

        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(ActionPossessor);
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            return new List<StateChange> {
                new ApplyDamageStateChange(Damage, args.Target)
            };
        }

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
                yield return new AttackActionArgs(unit);
        }
    }
}
