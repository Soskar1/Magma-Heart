using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class UnreachableTileDestructor : IRoomGenerator
    {
        private List<Vector2Int> m_directionsToVisit;

        public UnreachableTileDestructor()
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };
        }

        public void GenerateRoom(in RoomTileData roomTileData)
        {
            Dictionary<Vector2Int, DungeonTile> visitedTiles = new Dictionary<Vector2Int, DungeonTile>();
            Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
            tilesToVisit.Enqueue(roomTileData.WorldPosition);

            while (tilesToVisit.Count > 0)
            {
                Vector2Int tilePosition = tilesToVisit.Dequeue();
                DungeonTile tile = roomTileData.GetTile(tilePosition);
                visitedTiles.Add(tilePosition, tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTilePosition = tilePosition + direction;

                    if (roomTileData.ContainsTileAtPosition(neighbourTilePosition) && !visitedTiles.ContainsKey(neighbourTilePosition) && !tilesToVisit.Contains(neighbourTilePosition))
                        tilesToVisit.Enqueue(neighbourTilePosition);
                }
            }

            roomTileData.TileData.SetTiles(visitedTiles);
        }
    }
}