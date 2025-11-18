using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : UnitAction<EngageActionArgs>
    {
        private MoveAction m_moveAction;
        private AttackAction m_damageAction;
        private float m_speed;

        public EngageAction(AIUnit actionPossessor, float damage, float speed) : base(actionPossessor)
        {
            m_speed = speed;
            m_moveAction = new MoveAction(actionPossessor, speed);
            m_damageAction = new AttackAction(actionPossessor, damage);
        }

        public override List<StateChange> ProduceChanges(EngageActionArgs args, BoardState gameState)
        {
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            MoveActionArgs moveArgs = new MoveActionArgs(targetPosition.CurrentPosition);
            List<StateChange> movementChanges = m_moveAction.ProduceChanges(moveArgs, gameState);

            AttackActionArgs attackArgs = new AttackActionArgs(args.Target);
            List<StateChange> attackChanges = m_damageAction.ProduceChanges(attackArgs, gameState);

            return movementChanges.Concat(attackChanges).ToList();
        }

        public override bool CanExecute(EngageActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(ActionPossessor);
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > m_speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override ActionArgs CreateArgument(BoardState state, AIUnit unit) => new EngageActionArgs(unit);
    }
}
