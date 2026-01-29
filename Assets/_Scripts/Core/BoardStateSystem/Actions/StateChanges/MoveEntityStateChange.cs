using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record MoveEntityStateChange(int ExecutorId, List<Vector2> AStarPath) : MagmaHeartStateChange
    {
        // TODO: handel cancellationToken.Cancel
        public override async Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            if (AStarPath.Count == 1)
                return;

            List<RoomTile> roomTiles = new List<RoomTile>();

            foreach (Vector2 tile in AStarPath) {
                RoomTile roomTile = actualBoard.Room.GetRoomTile(tile);
                roomTiles.Add(roomTile);
            }

            actualBoard.Room.TryGetEntity(ExecutorId, out Entity entity);
            await actualBoard.Services.MovementService.MoveEntityAsync(entity, roomTiles, TileBasedMovement.DEFAULT_SPEED);
            UpdateBoard(actualBoard);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            UpdateBoard(simulation);

            simulation.Board.TryGetUnit(ExecutorId, out EntityModel model);
            model.TilePosition = AStarPath.Last().ToVector3Int();
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            Vector2 from = AStarPath.First();
            Vector2 to = AStarPath.Last();

            simulation.Board.TryGetUnit(ExecutorId, out EntityModel unit);
            simulation.Board.RemoveUnit(to);
            simulation.Board.AddUnit(from, unit);

            simulation.UpdateBoardNodeType(to, BoardNodeType.Walkable);
            simulation.UpdateBoardNodeType(from, BoardNodeType.Obstacle);

            unit.TilePosition = from.ToVector3Int();
        }

        private void UpdateBoard(BoardState boardState)
        {
            Vector2 from = AStarPath.First();
            Vector2 to = AStarPath.Last();

            boardState.Board.TryGetUnit(from, out AIUnitModel unit);
            boardState.Board.RemoveUnit(from);
            boardState.Board.AddUnit(to, unit);

            boardState.UpdateBoardNodeType(from, BoardNodeType.Walkable);
            boardState.UpdateBoardNodeType(to, BoardNodeType.Obstacle);
        }
    }
}
