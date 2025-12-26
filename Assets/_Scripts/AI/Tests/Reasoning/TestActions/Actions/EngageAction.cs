using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : UnitAction
    {
        private MoveAction m_moveAction;
        private AttackAction m_damageAction;

        public EngageAction()
        {
            m_moveAction = new MoveAction();
            m_damageAction = new AttackAction();
        }

        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            EngageActionArgs engageArgs = args as EngageActionArgs;

            Position targetPosition = gameState.GetProperty<Position>(engageArgs.Target);

            MoveActionArgs moveArgs = new MoveActionArgs(args.Executor, targetPosition, new MoveActionData(engageArgs.EngageActionData.Speed));
            IEnumerable<StateChange> movementChanges = m_moveAction.ProduceChanges(moveArgs, gameState);

            AttackActionArgs attackArgs = new AttackActionArgs(args.Executor, engageArgs.Target, new AttackActionData(engageArgs.EngageActionData.Damage));
            IEnumerable<StateChange> attackChanges = m_damageAction.ProduceChanges(attackArgs, gameState);

            return movementChanges.Concat(attackChanges).ToList();
        }

        public override bool CanExecute(ActionArgs args, BoardState gameState)
        {
            EngageActionArgs engageArgs = args as EngageActionArgs;

            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);
            Position targetPosition = gameState.GetProperty<Position>(engageArgs.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > engageArgs.EngageActionData.Speed + 1 || distance <= 1)
                return false;

            return true;
        }
    }
}
