using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TilePropagation : IRoomModifier
    {
        private readonly RoomData m_roomData;
        private readonly List<Vector2Int> m_directionsToVisit;
        private readonly int m_progagationLength;

        public TilePropagation(in RoomData roomData, in int propagationLength)
        {
            m_roomData = roomData;
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            m_progagationLength = propagationLength;
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

            HashSet<Vector2Int> newTiles = new HashSet<Vector2Int>();

            foreach (Vector2Int tile in tiles)
            {
                newTiles.Add(tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    for (int i = 1; i <= m_progagationLength; ++i)
                    {
                        Vector2Int newTile = m_roomData.ToRoomSpace(tile + direction * i);
                        newTiles.Add(newTile);
                    }
                }
            }

            return newTiles;
        }
    }
}