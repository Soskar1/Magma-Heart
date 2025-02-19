using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalkRoomGenerator
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

        public HashSet<Vector2Int> GenerateRoom(in Vector2Int startPosition)
        {
            HashSet<Vector2Int> path = new HashSet<Vector2Int>() { startPosition };

            Vector2Int currentPosition = startPosition;
            for (int i = 0; i < m_walkLength; ++i)
            {
                currentPosition += m_randomDirections[Random.Range(0, m_randomDirections.Count)];
                path.Add(currentPosition);
            }

            return path;
        }
    }
}