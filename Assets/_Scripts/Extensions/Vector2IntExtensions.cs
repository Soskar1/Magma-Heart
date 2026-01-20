using UnityEngine;

public static class Vector2IntExtension
{
    public static Vector2 ToVector2(this Vector2Int v) => new Vector2(v.x, v.y);
    public static Vector3 ToVector3(this Vector2Int v) => new Vector3(v.x, v.y);
    public static Vector3Int ToVector3Int(this Vector2Int v) => new Vector3Int(v.x, v.y);
}