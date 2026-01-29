using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record MoveCommand(int ExecutorId, Vector2Int From, Vector2Int To) : IBoardCommand
    {
        public void Execute(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel unit);
            board.RemoveUnit(From);
            board.AddUnit(To, unit);

            board.ChangeNodeType(From, BoardNodeType.Walkable);
            board.ChangeNodeType(To, BoardNodeType.Obstacle);

            unit.TilePosition = To.ToVector3Int();
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel unit);
            board.RemoveUnit(To);
            board.AddUnit(From, unit);

            board.ChangeNodeType(To, BoardNodeType.Walkable);
            board.ChangeNodeType(From, BoardNodeType.Obstacle);

            unit.TilePosition = From.ToVector3Int();
        }
    }
}
