using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
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

        public override IEnumerable<StateChange> ProduceChanges(MovementActionArgs args, BoardState boardState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, boardState);

            List<Vector2> path = CreatePath(args, boardState);

            return changes.Concat(new List<StateChange>()
            {
                new MoveEntityStateChange(args.TypedInput.TypedExecutor.Id, path)
            });
        }

        public override int GetEnergyCost(MovementActionArgs args, BoardState boardState)
        {
            List<Vector2> path = CreatePath(args, boardState);

            if (path == null || !path.Any())
                return int.MaxValue;

            int distance = path.Count - 1;

            return Mathf.CeilToInt(distance / (float)args.Speed);
        }

        public List<Vector2> CreatePath(MovementActionArgs args, BoardState boardState)
        {
            boardState.Board.TryGetUnit(args.Input.Executor.Id, out EntityModel entity);

            List<Vector2> path = m_aStar.FindPath(boardState.Board.Graph, entity.TilePosition.ToVector2Int(), args.TargetPositionInput.Target);

            if (path == null || !path.Any())
                return null;

            return path.Take(entity.Energy.CurrentEnergy * args.Speed + 1)
                .ToList();
        }

        public override bool CanExecute(MovementActionArgs args, BoardState boardState)
        {
            if (boardState.Board.GetNodeType(args.TargetPositionInput.Target) == BoardNodeType.Obstacle)
                return false;

            return base.CanExecute(args, boardState);
        }

        public override bool TryCreateArgs(TargetPositionActionInput input, MovementActionData data, BoardState boardState, out MovementActionArgs args)
        {
            boardState.Board.TryGetUnit(input.Executor.Id, out EntityModel entity);
            MovementActionArgs candidate = new MovementActionArgs(input, data.MovementDistanceInTilesForOneEnergy + entity.Speed.CurrentSpeed);

            if (!CanExecute(candidate, boardState))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, MovementActionData data, BoardState boardState, out ActionArgs args)
        {
            boardState.Board.TryGetUnit(executor.Id, out EntityModel entity);

            foreach (AIUnitModel potentialTarget in boardState.Board.GetUnits())
            {
                if (potentialTarget.Id == executor.Id)
                    continue;

                if (executor.IsPlayer && potentialTarget.IsPlayer)
                    continue;

                if (!executor.IsPlayer && !potentialTarget.IsPlayer)
                    continue;

                boardState.Board.TryGetUnit(potentialTarget.Id, out EntityModel targetUnit);
                Vector2 targetPosition = targetUnit.TilePosition.ToVector2();

                Vector2[] adjacentTiles =
                {
                    targetPosition + Vector2.up,
                    targetPosition + Vector2.down,
                    targetPosition + Vector2.left,
                    targetPosition + Vector2.right
                };

                List<Vector2> roomTiles =
                    adjacentTiles.Where(v => boardState.Board.Graph.ContainsNode(v) && boardState.Board.GetNodeType(v) == BoardNodeType.Walkable)
                    .OrderBy(t => RoomGrid.ManhattanDistance(entity.TilePosition, t.ToVector3Int()))
                    .ToList();

                foreach (Vector2 tile in roomTiles)
                {
                    //List<Vector2> path = m_aStar.FindPath(boardState.Board.Graph, source, tile);
                    //if (path == null || !path.Any())
                    //    continue;

                    //Vector2 targetTile = path.Skip(1).Take(energy.CurrentEnergy * data.MovementDistanceInTilesForOneEnergy).Last();
                    TargetPositionActionInput input = new TargetPositionActionInput(entity, tile);

                    if (TryCreateArgs(input, data, boardState, out args))
                        return true;
                }
            }

            args = null;
            return false;
        }
    }
}