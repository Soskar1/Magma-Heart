using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public interface IRoomGenerator
    {
        public HashSet<Vector2Int> GenerateRoom(in HashSet<Vector2Int> generatedTiles);
    }
}