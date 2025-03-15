using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class CurveGenerator
    {
        public List<Vector2> GenerateSmoothCurve(List<Vector2> points, int numPointsPerSegment)
        {
            List<Vector2> curve = new List<Vector2>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 p0 = (i == 0) ? points[i] : points[i - 1];
                Vector2 p1 = points[i];
                Vector2 p2 = points[i + 1];
                Vector2 p3 = (i + 2 < points.Count) ? points[i + 2] : points[i + 1];

                List<Vector2> segment = GenerateCubicBezier(p0, p1 + (p2 - p0) / 6, p2 - (p3 - p1) / 6, p2, numPointsPerSegment);
                curve.AddRange(segment);
            }

            return curve;
        }

        private List<Vector2> GenerateCubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int numPoints)
        {
            List<Vector2> curvePoints = new List<Vector2>();

            for (int i = 0; i <= numPoints; i++)
            {
                float t = i / (float)numPoints;
                Vector2 point = CubicBezierPoint(p0, p1, p2, p3, t);
                curvePoints.Add(point);
            }

            return curvePoints;
        }

        private Vector2 CubicBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1 - t;
            return (u * u * u) * p0 +
                (3 * u * u * t) * p1 +
                (3 * u * t * t) * p2 +
                (t * t * t) * p3;
        }

        
    }
}