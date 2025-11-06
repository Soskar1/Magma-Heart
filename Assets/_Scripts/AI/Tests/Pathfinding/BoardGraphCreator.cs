using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.AI.Pathifinding.Tests
{
    public static class BoardGraphCreator
    {
        // Grid-like graph
        public static (BoardGraph, Vector2[,]) Create3x3Graph()
        {
            BoardGraph graph = new BoardGraph();
            Vector2[,] nodePositions = new Vector2[3,3];
            for (int x = 0; x < 3; ++x)
            {
                for (int y = 0; y < 3; ++y)
                {
                    Vector2 nodePosition = new Vector2(x, y);

                    graph.AddNode(nodePosition, BoardNodeType.Walkable);
                    nodePositions[x, y] = nodePosition;
                }

                // Connect vertically
                for (int y = 0; y < 2; ++y)
                    graph.ConnectNodes(new Vector2(x, y), new Vector2(x, y + 1), 1);

                // Connect horizontally
                if (x > 0)
                    for (int y = 0; y < 3; ++y)
                        graph.ConnectNodes(new Vector2(x - 1, y), new Vector2(x, y), 1);
            }

            return (graph, nodePositions);
        }
    }
}