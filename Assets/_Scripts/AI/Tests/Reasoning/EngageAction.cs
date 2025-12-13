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

        public EngageAction(float damage, float speed)
        {
            m_speed = speed;
            m_moveAction = new MoveAction(speed);
            m_damageAction = new AttackAction(damage);
        }

        public override IEnumerable<StateChange> ProduceChanges(EngageActionArgs args, BoardState gameState)
        {
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            MoveActionArgs moveArgs = new MoveActionArgs(args.Executor, targetPosition.CurrentPosition);
            IEnumerable<StateChange> movementChanges = m_moveAction.ProduceChanges(moveArgs, gameState);

            AttackActionArgs attackArgs = new AttackActionArgs(args.Executor, args.Target);
            IEnumerable<StateChange> attackChanges = m_damageAction.ProduceChanges(attackArgs, gameState);

            return movementChanges.Concat(attackChanges).ToList();
        }

        public override bool CanExecute(EngageActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);
            Position targetPosition = gameState.GetProperty<Position>(args.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > m_speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
                yield return new EngageActionArgs(executor, unit);
        }
    }
}
