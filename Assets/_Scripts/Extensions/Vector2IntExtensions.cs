using UnityEngine;

public static class Vector2IntExtension
{
    public static Vector3 ToVector3(this Vector2Int v) => new Vector3(v.x, v.y);
}