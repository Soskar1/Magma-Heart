using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Bresenham;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackAction : CombatAction<AttackActionArgs, TargetEntityActionInput, AttackActionData>
    {
        public override int GetEnergyCost(AttackActionArgs args, BoardState boardState) => args.EnergyCost;

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState boardState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, boardState);

            return changes.Concat(new List<StateChange>
            {
                new AttackStateChange(args.TargetEntityInput.TypedExecutor.Id, args.TargetEntityInput.Target.Id, args.AttackDamage, args.AttackType),
            });
        }

        public override bool CanExecute(AttackActionArgs args, BoardState boardState)
        {
            bool result = base.CanExecute(args, boardState);
            if (!result)
                return result;

            PositionPropertySnapshot attackerPositionProperty = boardState.GetProperty<PositionPropertySnapshot>(args.TargetEntityInput.TypedExecutor);
            PositionPropertySnapshot targetPositionProperty = boardState.GetProperty<PositionPropertySnapshot>(args.TargetEntityInput.Target);

            if (attackerPositionProperty.ManhattanDistance(targetPositionProperty) > args.AttackDistance)
                return false;

            if (args.AttackType == AttackType.Ranged)
            {
                Vector2Int attackerPosition = attackerPositionProperty.Position.ToVector2Int();
                Vector2Int targetPosition = targetPositionProperty.Position.ToVector2Int();
                IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(attackerPosition, targetPosition);

                foreach (Vector2Int tile in tiles)
                {
                    bool isObstacle = boardState.Board.GetNodeType(tile) == BoardNodeType.Obstacle;

                    if (isObstacle && tile != attackerPosition && tile != targetPosition)
                        return false;
                }
            }

            return true;
        }

        public override bool TryCreateArgs(TargetEntityActionInput input, AttackActionData data, BoardState boardState, out AttackActionArgs args)
        {
            EntityModel executor = input.TypedExecutor;
            EntityModel target = input.Target;
            args = null;

            if (target.Id == executor.Id)
                return false;

            if (executor.IsPlayer && target.IsPlayer)
                return false;

            if (!executor.IsPlayer && !target.IsPlayer)
                return false;

            StrengthPropertySnapshot strength = boardState.GetProperty<StrengthPropertySnapshot>(executor);
            int attackDamage = data.AttackDamage + strength.Strength;

            AttackActionArgs candidate = new AttackActionArgs(input, data.EnergyCost, data.AttackDistance, attackDamage, data.AttackType);

            if (!CanExecute(candidate, boardState))
                return false;

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, AttackActionData data, BoardState boardState, out ActionArgs args)
        {
            foreach (AIUnitModel unit in boardState.Board.GetUnits())
            {
                TargetEntityActionInput input = new TargetEntityActionInput((EntityModel)executor, (EntityModel)unit);

                if (TryCreateArgs(input, data, boardState, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}