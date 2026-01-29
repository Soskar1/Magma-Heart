using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementAction : CombatAction<MovementActionArgs, TargetPositionActionInput, MovementActionData>
    {
        private readonly AStar m_aStar;

        public MovementAction()
        {
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public override IEnumerable<IBoardCommand> Execute(MovementActionArgs args, Board board)
        {
            IEnumerable<IBoardCommand> changes = base.Execute(args, board);

            List<Vector2> path = CreatePath(args, board);

            return changes.Concat(new List<IBoardCommand>()
            {
                new MoveCommand(args.TypedInput.TypedExecutor.Id, path)
            });
        }

        public override int GetEnergyCost(MovementActionArgs args, Board board)
        {
            List<Vector2> path = CreatePath(args, board);

            if (path == null || !path.Any())
                return int.MaxValue;

            int distance = path.Count - 1;

            return Mathf.CeilToInt(distance / (float)args.Speed);
        }

        public List<Vector2> CreatePath(MovementActionArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out EntityModel entity);

            List<Vector2> path = m_aStar.FindPath(board.Graph, entity.TilePosition.ToVector2Int(), args.TargetPositionInput.Target);

            if (path == null || !path.Any())
                return null;

            return path.Take(entity.Energy.CurrentEnergy * args.Speed + 1)
                .ToList();
        }

        public override bool CanExecute(MovementActionArgs args, Board board)
        {
            if (board.GetNodeType(args.TargetPositionInput.Target) == BoardNodeType.Obstacle)
                return false;

            return base.CanExecute(args, board);
        }

        public override bool TryCreateArgs(TargetPositionActionInput input, MovementActionData data, Board board, out MovementActionArgs args)
        {
            board.TryGetUnit(input.Executor.Id, out EntityModel entity);
            MovementActionArgs candidate = new MovementActionArgs(input, data.MovementDistanceInTilesForOneEnergy + entity.Speed.CurrentSpeed);

            if (!CanExecute(candidate, board))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, MovementActionData data, Board board, out ActionArgs args)
        {
            board.TryGetUnit(executor.Id, out EntityModel entity);

            foreach (AIUnitModel potentialTarget in board.GetUnits())
            {
                if (potentialTarget.Id == executor.Id)
                    continue;

                if (executor.IsPlayer && potentialTarget.IsPlayer)
                    continue;

                if (!executor.IsPlayer && !potentialTarget.IsPlayer)
                    continue;

                board.TryGetUnit(potentialTarget.Id, out EntityModel targetUnit);
                Vector2 targetPosition = targetUnit.TilePosition.ToVector2();

                Vector2[] adjacentTiles =
                {
                    targetPosition + Vector2.up,
                    targetPosition + Vector2.down,
                    targetPosition + Vector2.left,
                    targetPosition + Vector2.right
                };

                List<Vector2> roomTiles =
                    adjacentTiles.Where(v => board.Graph.ContainsNode(v) && board.GetNodeType(v) == BoardNodeType.Walkable)
                    .OrderBy(t => RoomGrid.ManhattanDistance(entity.TilePosition, t.ToVector3Int()))
                    .ToList();

                foreach (Vector2 tile in roomTiles)
                {
                    //List<Vector2> path = m_aStar.FindPath(board.Board.Graph, source, tile);
                    //if (path == null || !path.Any())
                    //    continue;

                    //Vector2 targetTile = path.Skip(1).Take(energy.CurrentEnergy * data.MovementDistanceInTilesForOneEnergy).Last();
                    TargetPositionActionInput input = new TargetPositionActionInput(entity, tile);

                    if (TryCreateArgs(input, data, board, out args))
                        return true;
                }
            }

            args = null;
            return false;
        }
    }
}