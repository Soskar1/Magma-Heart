using UnityEngine;

namespace MagmaHeart.Extensions
{
    public static class Vector3IntExtension
    {
        public static Vector2 ToVector2(this Vector3Int v) => new Vector2(v.x, v.y);
        public static Vector2Int ToVector2Int(this Vector3Int v) => new Vector2Int(v.x, v.y);
    }
}
