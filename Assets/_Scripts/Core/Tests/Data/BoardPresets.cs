using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal static class BoardPresets
    {
        public static Board CreateEmptyBoard(BoardDimensions dimensions)
        {
            BoardGraph graph = new BoardGraph();
            for (int x = dimensions.StartPoint.x; x < dimensions.EndPoint.x; ++x)
            {
                for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y; ++y)
                    graph.AddNode(new Vector2(x, y), BoardNodeType.Walkable);

                for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y - 1; ++y)
                    graph.ConnectNodes(new Vector2(x, y), new Vector2(x, y + 1), 1);

                if (x > 0)
                    for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y; ++y)
                        graph.ConnectNodes(new Vector2(x - 1, y), new Vector2(x, y), 1);
            }

            return new Board(graph);
        }
    }
}