using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalkRoomGenerator : IRoomGenerator
    {
        private int m_walkLength = 0;
        private List<Vector2Int> m_randomDirections;

        public RandomWalkRoomGenerator(int walkLength)
        {
            m_walkLength = walkLength;
            m_randomDirections = new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down
            };
        }

        public HashSet<Vector2Int> GenerateRoom(in RoomData roomData, in Vector2Int startPosition)
        {
            HashSet<Vector2Int> path = new HashSet<Vector2Int>() { startPosition };

            Vector2Int currentPosition = startPosition;
            for (int i = 0; i < m_walkLength; ++i)
            {
                currentPosition += PickRandomDirection(roomData, currentPosition);
                path.Add(currentPosition);
            }

            return path;
        }

        private Vector2Int PickRandomDirection(in RoomData roomData, in Vector2Int currentPosition)
        {
            m_randomDirections.Shuffle();
            for (int i = 0; i < m_randomDirections.Count; ++i)
                if (roomData.IsInRoomSpace(currentPosition + m_randomDirections[i]))
                    return m_randomDirections[i];

            Debug.LogWarning("[RandomWalkRoomGenerator] PickRandomDirection does not take the right direction. Returning Vector2Int.zero");
            return Vector2Int.zero;
        }
    }
}