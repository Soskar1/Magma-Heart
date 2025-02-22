using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RandomWalk
    {
        private readonly List<Vector2Int> m_directions;

        public RandomWalk(in List<Vector2Int> directions) => m_directions = directions;

        public Vector2Int TakeRandomDirection() => m_directions[Random.Range(0, m_directions.Count)];
    }
}