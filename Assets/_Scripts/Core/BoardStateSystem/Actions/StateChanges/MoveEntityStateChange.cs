using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record MoveEntityStateChange(EntityModel EntityModel, List<Vector2> AStarPath) : MagmaHeartStateChange
    {
        public override void ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            List<RoomTile> roomTiles = new List<RoomTile>();

            foreach (Vector2 tile in AStarPath) {
                RoomTile roomTile = actualBoard.Board.GetRoomTile(tile);
                roomTiles.Add(roomTile);
            }

            actualBoard.MovementService.MoveEntity(EntityModel.Entity, roomTiles);
            UpdateBoard(actualBoard);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            UpdateBoard(simulation);
        }

        private void UpdateBoard(BoardState boardState)
        {
            Vector2 from = AStarPath.First();
            Vector2 to = AStarPath.Last();
            boardState.RemoveUnit(from, EntityModel);
            boardState.AddUnit(to, EntityModel);
            boardState.UpdateBoardNodeType(from, BoardNodeType.Walkable);
            boardState.UpdateBoardNodeType(to, BoardNodeType.Obstacle);
        }
    }
}
