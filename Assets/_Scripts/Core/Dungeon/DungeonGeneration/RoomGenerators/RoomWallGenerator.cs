using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomWallGenerator : IRoomGenerator
    {
        private readonly RandomWalk m_randomWalk;
        private readonly Random m_random;
        private readonly int m_maxWallLength;
        private readonly int m_amountOfWalls;

        public RoomWallGenerator(in Random random, in int amountOfWalls, in int maxWallLength)
        {
            m_randomWalk = new RandomWalk(random, new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down
            });

            m_random = random;
            m_maxWallLength = maxWallLength;
            m_amountOfWalls = amountOfWalls;
        }

        public void GenerateRoom(in RoomTileData RoomTileData)
        {
            for (int wall = 0; wall < m_amountOfWalls; ++wall)
            {
                Vector2Int currentPosition = RoomTileData.GetTilePositionAtIndex(m_random.Next(RoomTileData.TileCount));
                Vector2Int wallDirection = m_randomWalk.TakeRandomDirection();

                for (int i = 0; i < m_maxWallLength; ++i)
                {
                    Vector2Int wallPosition = currentPosition + wallDirection * i;
                    RoomTileData.AddTile(wallPosition, TileType.Wall);
                }
            }
        }
    }
}