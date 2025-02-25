using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class UnreachableTileDesctructor : IRoomModifier
    {
        private readonly RoomData m_roomData;
        private List<Vector2Int> m_directionsToVisit;

        public UnreachableTileDesctructor(RoomData roomData)
        {
            m_roomData = roomData;
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };
        }

        public HashSet<Vector2Int> ModifyRoom(in HashSet<Vector2Int> tiles)
        {
            if (tiles == null)
            {
                Debug.LogWarning("tiles is null. Returning new empty HashSet object");
                return new HashSet<Vector2Int>();
            }

            if (tiles.Count == 0)
            {
                Debug.LogWarning("tiles is empty. Terminating job");
                return tiles;
            }

            HashSet<Vector2Int> visitedTiles = new HashSet<Vector2Int>();
            Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
            tilesToVisit.Enqueue(m_roomData.WorldPosition);

            while (tilesToVisit.Count > 0)
            {
                Vector2Int tile = tilesToVisit.Dequeue();
                visitedTiles.Add(tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTile = tile + direction;

                    if (tiles.Contains(neighbourTile) && !visitedTiles.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                        tilesToVisit.Enqueue(neighbourTile);
                }
            }

            return visitedTiles;
        }
    }
}