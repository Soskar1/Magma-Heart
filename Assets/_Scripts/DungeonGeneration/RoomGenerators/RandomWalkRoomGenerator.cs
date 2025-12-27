using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.DungeonGeneration.RoomGeneration
{
    public class RandomWalkRoomGenerator : IRoomGenerator
    {
        private readonly RandomWalk m_randomWalk;
        private readonly int m_randomWalkIterations = 0;
        private readonly Random m_random;

        public RandomWalkRoomGenerator(in Random random, in int randomWalkIterations)
        {
            m_randomWalkIterations = randomWalkIterations;
            m_randomWalk = new RandomWalk(random, new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down
            });

            m_random = random;
        }

        public void GenerateRoom(in RoomModel roomModel)
        {
            Vector2Int currentPosition = roomModel.WorldPosition;

            if (roomModel.TileCount > 1)
                currentPosition = roomModel.GetTilePositionAtIndex(m_random.Next(roomModel.TileCount));

            for (int i = 0; i < m_randomWalkIterations; ++i)
            {
                Vector2Int newPosition = roomModel.ToRoomSpace(currentPosition + m_randomWalk.TakeRandomDirection());
                currentPosition = newPosition;
                roomModel.AddTile(currentPosition, TileType.Floor);
            }
        }
    }
}