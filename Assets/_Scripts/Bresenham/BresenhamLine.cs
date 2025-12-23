using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Bresenham
{
    public static class BresenhamLine
    {
        public static List<Vector2Int> DrawLine(Vector2Int start, Vector2Int end)
        {
            if (Mathf.Abs(end.x - start.x) > Mathf.Abs(end.y - start.y))
                return DrawHorizontalLine(start, end);
            else
                return DrawVerticalLine(start, end);
        }

        private static List<Vector2Int> DrawHorizontalLine(Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> points = new List<Vector2Int>();

            if (start.x > end.x)
            {
                Vector2Int tmp = start;
                start = end;
                end = tmp;
            }

            Vector2Int difference = end - start;
            int direction = difference.y < 0 ? -1 : 1;
            difference.y *= direction;

            if (difference.x != 0)
            {
                int currentY = start.y;
                int distanceDifference = 2 * difference.y - difference.x;
                for (int i = 0; i <= difference.x; ++i)
                {
                    Vector2Int point = new Vector2Int(start.x + i, currentY);
                    points.Add(point);

                    if (distanceDifference >= 0)
                    {
                        currentY += direction;
                        distanceDifference -= 2 * difference.x;
                    }
                    distanceDifference += 2 * difference.y;
                }
            }

            return points;
        }

        private static List<Vector2Int> DrawVerticalLine(Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> points = new List<Vector2Int>();

            if (start.y > end.y)
            {
                Vector2Int tmp = start;
                start = end;
                end = tmp;
            }

            Vector2Int difference = end - start;
            int direction = difference.x < 0 ? -1 : 1;
            difference.x *= direction;

            if (difference.y != 0)
            {
                int currentX = start.x;
                int distanceDifference = 2 * difference.x - difference.y;
                for (int i = 0; i <= difference.y; ++i)
                {
                    Vector2Int point = new Vector2Int(currentX, start.y + i);
                    points.Add(point);

                    if (distanceDifference >= 0)
                    {
                        currentX += direction;
                        distanceDifference -= 2 * difference.y;
                    }
                    distanceDifference += 2 * difference.x;
                }
            }

            return points;
        }
    }
}