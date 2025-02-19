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

        public HashSet<Vector2Int> GenerateRoom(in RoomData roomData, in Vector2Int startPosition)
        {
            HashSet<Vector2Int> path = new HashSet<Vector2Int>() { startPosition };

            Vector2Int currentPosition = startPosition;
            for (int i = 0; i < m_walkLength; ++i)
            {
                Vector2Int randomDirection = PickRandomDirection();
                while (!IsInRoomSpace(roomData, currentPosition + randomDirection))
                {
                    Debug.Log($"{currentPosition + randomDirection} is out of bounds");
                    randomDirection = PickRandomDirection();
                }

                currentPosition += randomDirection;
                path.Add(currentPosition);
            }

            return path;
        }

        private Vector2Int PickRandomDirection() => m_randomDirections[Random.Range(0, m_randomDirections.Count)];

        private bool IsInRoomSpace(in RoomData roomData, in Vector2Int position)
        {
            return position.x < roomData.RightBorder && position.x > roomData.LeftBorder &&
                position.y < roomData.UpperBorder && position.y > roomData.BottomBorder;
        }
    }
}