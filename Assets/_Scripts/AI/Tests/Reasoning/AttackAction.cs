using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs>
    {
        public float Damage { get; init; }

        public AttackAction(AIUnit actionPossessor, float damage) : base(actionPossessor)
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

        public override IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnit unit) => new List<AttackActionArgs>() { new AttackActionArgs(unit) };
    }
}
