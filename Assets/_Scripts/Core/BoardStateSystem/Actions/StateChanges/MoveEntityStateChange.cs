using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record MoveEntityStateChange(EntityModel EntityModel, List<Vector2> AStarPath) : MagmaHeartStateChange
    {
        // TODO: handel cancellationToken.Cancel
        public override async Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            List<RoomTile> roomTiles = new List<RoomTile>();

            foreach (Vector2 tile in AStarPath) {
                RoomTile roomTile = actualBoard.Room.GetRoomTile(tile);
                roomTiles.Add(roomTile);
            }

            actualBoard.Room.TryGetEntity(EntityModel, out Entity entity);
            await actualBoard.MovementService.MoveEntityAsync(entity, roomTiles, TileBasedMovement.DEFAULT_SPEED);
            UpdateBoard(actualBoard);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            UpdateBoard(simulation);

            PositionPropertySnapshot position = new PositionPropertySnapshot(AStarPath.Last().ToVector3Int());
            simulation.UpdateProperty(EntityModel, position);
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
