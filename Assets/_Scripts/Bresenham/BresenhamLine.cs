using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Bresenham
{
    public static class BresenhamLine
    {
        public static List<Vector2Int> DrawLine(Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> points = new List<Vector2Int>();
            
            Vector2Int diff = end - start;
            int step = Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y));

            if (step != 0)
            {
                float stepX = diff.x / (float)step;
                float stepY = diff.y / (float)step;

                for (int i = 0; i <= step; ++i)
                {
                    int x = Mathf.RoundToInt(start.x + i * stepX);
                    int y = Mathf.RoundToInt(start.y + i * stepY);
                    Vector2Int point = new Vector2Int(x, y);
                    points.Add(point);
                }
            }

            return points;
        }
    }
}