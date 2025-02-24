using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public interface IRoomModifier
    {
        public HashSet<Vector2Int> ModifyRoom(in HashSet<Vector2Int> tiles);
    }
}