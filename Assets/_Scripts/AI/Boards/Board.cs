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

        internal SimulatedBoard CreateSimulatedBoard()
        {
            BoardGraph graphCopy = Graph.DeepCopy();
            return new SimulatedBoard(graphCopy, Units);
        }
    }
}
