using MagmaHeart.BreadthFirstSearch;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Navigation;
using System.Linq;

namespace MagmaHeart.Core.Navigation
{
    public static class AStarGraphBuilder
    {
        public static AStarGraph GenerateAStarGraph(RoomTileData roomTileData)
        {
            AStarGraph graph = new AStarGraph();

            foreach (DungeonTile tile in roomTileData)
            {
                AStarNode node = new AStarNode(tile.Position, AStarNodeType.None);

                if (tile.Type == TileType.Floor)
                    node.Type = AStarNodeType.Walkable;
                else
                    node.Type = AStarNodeType.Obstacle;

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