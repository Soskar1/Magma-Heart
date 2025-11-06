using MagmaHeart.AI.Pathifinding;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }

        public Board(BoardGraph graph) => Graph = graph;

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);
    }
}
