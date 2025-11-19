using MagmaHeart.BreadthFirstSearch;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.AI.Boards;
using System.Linq;

namespace MagmaHeart.Core.BoardStateSystem
{
    public static class BoardGraphBuilder
    {
        public static BoardGraph GenerateBoardGraph(RoomTileData roomTileData)
        {
            BoardGraph graph = new BoardGraph();

            foreach (DungeonTile tile in roomTileData)
            {
                BoardNode node = new BoardNode(tile.Position, BoardNodeType.None);

                if (tile.Type == TileType.Floor)
                    node.Type = BoardNodeType.Walkable;
                else
                    node.Type = BoardNodeType.Obstacle;

                graph.AddNode(node);
            }

            BreadthFirstSearch<DungeonTile> bfs = new BreadthFirstSearch<DungeonTile>(roomTileData.GetAdjacentTiles, (sourceNode, adjacentNode) =>
            {
                graph.ConnectNodes(sourceNode.Position, adjacentNode.Position, 1); // TODO: use cost in the future
            });

            bfs.Perform(roomTileData.First());

            return graph;
        }
    }
}