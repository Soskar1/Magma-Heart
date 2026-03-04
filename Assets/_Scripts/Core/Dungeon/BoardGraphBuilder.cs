using MagmaHeart.BreadthFirstSearch;
using MagmaHeart.AI.Boards;
using System.Linq;
using MagmaHeart.DungeonGeneration;

namespace MagmaHeart.Core.Dungeon
{
    public static class BoardGraphBuilder
    {
        public static BoardGraph GenerateBoardGraph(RoomModel roomModel)
        {
            BoardGraph graph = new BoardGraph();

            foreach (DungeonTile tile in roomModel)
            {
                BoardNode node = new BoardNode(tile.Position, BoardNodeType.None);

                if (tile.Type == TileType.Floor)
                    node.Type = BoardNodeType.Walkable;
                else
                    node.Type = BoardNodeType.Obstacle;

                graph.AddNode(node);
            }

            BreadthFirstSearch<DungeonTile> bfs = new BreadthFirstSearch<DungeonTile>(roomModel.GetAdjacentTiles, (sourceNode, adjacentNode) =>
            {
                graph.ConnectNodes(sourceNode.Position, adjacentNode.Position, 1); // TODO: use cost in the future
            });

            bfs.Perform(roomModel.First());

            return graph;
        }
    }
}