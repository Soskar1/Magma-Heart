using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalkRoomGenerator : IRoomGenerator
    {
        private readonly RoomData m_roomData;
        private readonly RandomWalk m_randomWalk;
        private readonly int m_randomWalkIterations = 0;
        private readonly Random m_random;

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

            m_random = new Random();
        }

        public HashSet<Vector2Int> GenerateRoom(in HashSet<Vector2Int> generatedTiles)
        {
            HashSet<Vector2Int> tiles = new HashSet<Vector2Int>() { m_roomData.WorldPosition };
            Vector2Int currentPosition = m_roomData.WorldPosition;

            if (generatedTiles != null)
            {
                tiles = generatedTiles;
                currentPosition = generatedTiles.ElementAt(m_random.Next(generatedTiles.Count));
            }

            for (int i = 0; i < m_randomWalkIterations; ++i)
            {
                Vector2Int newPosition = m_roomData.ToRoomSpace(currentPosition + m_randomWalk.TakeRandomDirection());
                currentPosition = newPosition;
                tiles.Add(currentPosition);
            }

            return tiles;
        }
    }
}