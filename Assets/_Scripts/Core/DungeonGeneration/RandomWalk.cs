using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalk
    {
        private readonly List<Vector2Int> m_directions;
        private readonly Random m_random;

        public RandomWalk(in List<Vector2Int> directions)
        {
            m_directions = directions;
            m_random = new Random();
        }

        public Vector2Int TakeRandomDirection() => m_directions[m_random.Next(m_directions.Count)];
    }
}