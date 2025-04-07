using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomWallGenerator : IRoomGenerator
    {
        private readonly RandomWalk m_randomWalk;
        private readonly Random m_random;
        private readonly int m_wallThickness;
        private readonly int m_maxWallLength;

        public RoomWallGenerator(in Random random, in int wallThickness, in int maxWallLength)
        {
            m_randomWalk = new RandomWalk(random, new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down
            });

            m_random = random;
            m_wallThickness = wallThickness;
            m_maxWallLength = maxWallLength;
        }

        public void GenerateRoom(in RoomData roomData)
        {
            //Vector2Int currentPosition = roomData.GetTileAtIndex(m_random.Next(roomData.TileCount));
            //Vector2Int wallDirection = m_randomWalk.TakeRandomDirection();

            //for (int i = 0; i < m_maxWallLength; ++i)
            //{
            //    Vector2Int newPosition = roomData.ToRoomSpace(currentPosition + wallDirection);
            //    currentPosition = newPosition;
            //    roomData.AddTile(currentPosition);
            //}
        }
    }
}