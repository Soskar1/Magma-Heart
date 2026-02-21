using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Bresenham;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.BoardStateSystem.Actions.Commands;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackAction : CombatAction<AttackActionArgs, TargetEntityActionInput, AttackActionData>
    {
        public override int GetEnergyCost(AttackActionArgs args, Board board) => args.EnergyCost;

        public override IEnumerable<IBoardCommand> Execute(AttackActionArgs args, Board board)
        {
            IEnumerable<IBoardCommand> changes = base.Execute(args, board);

            return changes.Concat(new List<IBoardCommand>
            {
                new ApplyDamageCommand(args.Input.Executor.Id, args.TargetEntityInput.Target.Id, args.AttackDamage, args.AttackType)
            });
        }

        public override bool CanExecute(AttackActionArgs args, Board board)
        {
            bool result = base.CanExecute(args, board);
            if (!result)
                return result;

            board.TryGetUnit(args.TargetEntityInput.TypedExecutor.Id, out EntityModel attacker);
            board.TryGetUnit(args.TargetEntityInput.Target.Id, out EntityModel target);

            if (WorldGrid.ManhattanDistance(attacker.TilePosition, target.TilePosition) > args.AttackDistance)
                return false;

            if (args.AttackType == AttackType.Ranged)
            {
                Vector2Int attackerPosition = attacker.TilePosition.ToVector2Int();
                Vector2Int targetPosition = target.TilePosition.ToVector2Int();
                IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(attackerPosition, targetPosition);

                foreach (Vector2Int tile in tiles)
                {
                    bool isObstacle = board.GetNodeType(tile) == BoardNodeType.Obstacle;

                    if (isObstacle && tile != attackerPosition && tile != targetPosition)
                        return false;
                }
            }

            return true;
        }

        public override bool TryCreateArgs(TargetEntityActionInput input, AttackActionData data, Board board, out AttackActionArgs args)
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

            board.TryGetUnit(executor.Id, out EntityModel attacker);
            int attackDamage = data.AttackDamage + attacker.Strength.CurrentStrength;

            AttackActionArgs candidate = new AttackActionArgs(input, data.EnergyCost, data.AttackDistance, attackDamage, data.AttackType);

            if (!CanExecute(candidate, board))
                return false;

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, AttackActionData data, Board board, out ActionArgs args)
        {
            foreach (AIUnitModel unit in board.GetUnits())
            {
                TargetEntityActionInput input = new TargetEntityActionInput((EntityModel)executor, (EntityModel)unit);

                if (TryCreateArgs(input, data, board, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}