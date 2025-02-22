using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalkRoomGenerator : IRoomGenerator
    {
        private readonly RoomData m_roomData;
        private readonly RandomWalk m_randomWalk;
        private readonly int m_randomWalkIterations = 0;

        public RandomWalkRoomGenerator(in RoomData roomData, in int randomWalkIterations)
        {
            m_randomWalkIterations = randomWalkIterations;
            m_randomWalk = new RandomWalk(new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down
            });
            m_roomData = roomData;
        }

        public HashSet<Vector2Int> GenerateRoom(in Vector2Int startPosition)
        {
            HashSet<Vector2Int> path = new HashSet<Vector2Int>() { startPosition };

            Vector2Int currentPosition = startPosition;
            for (int i = 0; i < m_randomWalkIterations; ++i)
            {
                Vector2Int newPosition = currentPosition + m_randomWalk.TakeRandomDirection();

                if (newPosition.x > m_roomData.RightBorder)
                    newPosition.x = m_roomData.RightBorder;
                
                if (newPosition.x < m_roomData.LeftBorder)
                    newPosition.x = m_roomData.LeftBorder;
                
                if (newPosition.y > m_roomData.UpperBorder)
                    newPosition.y = m_roomData.UpperBorder;
                
                if (newPosition.y < m_roomData.BottomBorder)
                    newPosition.y = m_roomData.BottomBorder;

                currentPosition = newPosition;
                path.Add(currentPosition);
            }

            return path;
        }
    }
}