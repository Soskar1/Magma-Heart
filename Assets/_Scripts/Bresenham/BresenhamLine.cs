using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Bresenham
{
    // Reference: https://www.youtube.com/watch?v=CceepU1vIKo
    public static class BresenhamLine
    {
        public static IEnumerable<Vector2Int> DrawLine(Vector2Int start, Vector2Int end)
        {
            if (Mathf.Abs(end.x - start.x) > Mathf.Abs(end.y - start.y))
                return DrawHorizontalLine(start, end);
            else
                return DrawVerticalLine(start, end);
        }

        private static IEnumerable<Vector2Int> DrawHorizontalLine(Vector2Int start, Vector2Int end)
        {
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
                int decision = 2 * difference.y - difference.x;
                for (int i = 0; i <= difference.x; ++i)
                {
                    yield return new Vector2Int(start.x + i, currentY);

                    if (decision >= 0)
                    {
                        currentY += direction;
                        decision -= 2 * difference.x;
                    }
                    decision += 2 * difference.y;
                }
            }
        }

        private static IEnumerable<Vector2Int> DrawVerticalLine(Vector2Int start, Vector2Int end)
        {
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
                int decision = 2 * difference.x - difference.y;
                for (int i = 0; i <= difference.y; ++i)
                {
                    yield return new Vector2Int(currentX, start.y + i);

                    if (decision >= 0)
                    {
                        currentX += direction;
                        decision -= 2 * difference.y;
                    }
                    decision += 2 * difference.x;
                }
            }
        }
    }
}