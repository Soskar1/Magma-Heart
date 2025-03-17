using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TilePropagation : IRoomModifier
    {
        private readonly List<Vector2Int> m_directionsToVisit;
        private readonly int m_progagationLength;

        public TilePropagation(in int propagationLength)
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            m_progagationLength = propagationLength;
        }

        public void ModifyRoom(in RoomData roomData)
        {
            HashSet<Vector2Int> tilesToVisit = roomData.GetTilesCopy();

            foreach (Vector2Int tile in tilesToVisit)
                foreach (Vector2Int direction in m_directionsToVisit)
                    for (int i = 1; i <= m_progagationLength; ++i)
                        roomData.AddTile(tile + direction * i);
        }
    }
}