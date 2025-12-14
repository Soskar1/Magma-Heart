using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : UnitAction<EngageActionArgs, EngageActionPayload>
    {
        private MoveAction m_moveAction;
        private AttackAction m_damageAction;

        public EngageAction()
        {
            m_moveAction = new MoveAction();
            m_damageAction = new AttackAction();
        }

        public override IEnumerable<StateChange> ProduceChanges(EngageActionArgs args, BoardState gameState)
        {
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            MoveActionArgs moveArgs = new MoveActionArgs(args.Executor, targetPosition.CurrentPosition, args.Speed);
            IEnumerable<StateChange> movementChanges = m_moveAction.ProduceChanges(moveArgs, gameState);

            AttackActionArgs attackArgs = new AttackActionArgs(args.Executor, args.Target, args.Damage);
            IEnumerable<StateChange> attackChanges = m_damageAction.ProduceChanges(attackArgs, gameState);

            return movementChanges.Concat(attackChanges).ToList();
        }

        public override bool CanExecute(EngageActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > args.Speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, EngageActionPayload payload, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
                yield return new EngageActionArgs(executor, unit, payload.Damage, payload.Speed);
        }
    }
}
