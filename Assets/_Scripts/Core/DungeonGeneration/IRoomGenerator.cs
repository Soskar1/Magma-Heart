using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core
{
    public interface IRoomGenerator
    {
        public HashSet<Vector2Int> GenerateRoom(in Vector2Int startPosition);
    }
}