using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }
        public Dictionary<Vector2, AIUnit> Units { get; init; }

        public Board(BoardGraph graph)
        {
            Graph = graph;
            Units = new Dictionary<Vector2, AIUnit>();
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);

        public Board DeepCopy()
        {
            BoardGraph graph = Graph.DeepCopy();
            Board board = new Board(graph);
            // TODO
            return board;
        }
    }
}
