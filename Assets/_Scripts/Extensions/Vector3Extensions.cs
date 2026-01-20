using UnityEngine;

public static class Vector3Extension
{
    public static Vector2Int ToVector2Int(this Vector3 v) => new Vector2Int((int)v.x, (int)v.y);
    public static Vector3Int ToVector3Int(this Vector3 v) => new Vector3Int((int)v.x, (int)v.y, (int)v.z);
}