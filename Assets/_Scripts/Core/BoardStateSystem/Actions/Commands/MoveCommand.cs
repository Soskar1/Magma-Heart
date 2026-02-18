using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Commands
{
    public record MoveCommand(int ExecutorId, List<Vector2> Path) : IBoardCommand
    {
        public void Execute(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel unit);
            board.RemoveUnit(Path.First());
            board.AddUnit(Path.Last(), unit);

            board.ChangeNodeType(Path.First(), BoardNodeType.Walkable);
            board.ChangeNodeType(Path.Last(), BoardNodeType.Obstacle);

            unit.TilePosition = Path.Last().ToVector3Int();
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(ExecutorId, out EntityModel unit);
            board.RemoveUnit(Path.Last());
            board.AddUnit(Path.First(), unit);

            board.ChangeNodeType(Path.Last(), BoardNodeType.Walkable);
            board.ChangeNodeType(Path.First(), BoardNodeType.Obstacle);

            unit.TilePosition = Path.First().ToVector3Int();
        }
    }
}
