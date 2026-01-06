using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : UnitAction<EngageActionArgs, TargetEntityActionInput, EngageActionData>
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
            Position targetPosition = gameState.GetProperty<Position>(args.TypedInput.Target);

            TargetPositionActionInput movementInput = new TargetPositionActionInput(args.Input.Executor, targetPosition);
            MoveActionArgs moveArgs = new MoveActionArgs(movementInput, new MoveActionData(args.EngageActionData.Speed));
            IEnumerable<StateChange> movementChanges = m_moveAction.ProduceChanges(moveArgs, gameState);

            AttackActionArgs attackArgs = new AttackActionArgs(args.TypedInput, new AttackActionData(args.EngageActionData.Damage));
            IEnumerable<StateChange> attackChanges = m_damageAction.ProduceChanges(attackArgs, gameState);

            return movementChanges.Concat(attackChanges).ToList();
        }

        public override bool CanExecute(EngageActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Input.Executor);
            Position targetPosition = gameState.GetProperty<Position>(args.TypedInput.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > args.EngageActionData.Speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override bool TryCreateArgs(TargetEntityActionInput input, EngageActionData data, BoardState boardState, out EngageActionArgs args)
        {
            EngageActionArgs candidate = new EngageActionArgs(input, data);

            if (!CanExecute(candidate, boardState))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, EngageActionData data, BoardState boardState, out EngageActionArgs args)
        {
            foreach (AIUnitModel unit in boardState.Board.GetUnits())
            {
                if (unit == executor)
                    continue;

                TargetEntityActionInput input = new TargetEntityActionInput(executor, unit);
                if (TryCreateArgs(input, data, boardState, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}
