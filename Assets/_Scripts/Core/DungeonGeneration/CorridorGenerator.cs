using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class CorridorGenerator
    {
        private readonly CurveGenerator m_curveGenerator;
        private List<Vector2Int> m_tilesToGrab;
        private const int OFFSET = 4;
        private const int INITIAL_POINTS = 10;
        private readonly Random m_random;

        public CorridorGenerator(in Random random, in int corridorSize)
        {
            m_curveGenerator = new CurveGenerator();
            m_tilesToGrab = new List<Vector2Int>();
            m_random = random;

            for (int x = -corridorSize; x <= corridorSize; x++)
                for (int y = -corridorSize; y <= corridorSize; y++)
                    if (x * x + y * y <= corridorSize * corridorSize)
                        m_tilesToGrab.Add(new Vector2Int(x, y));
        }

        public HashSet<Vector2Int> GenerateCorridor(in RoomData room1, in RoomData room2)
        {
            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();
            Vector2 direction = ((Vector2)(room1.WorldPosition - room2.WorldPosition)).normalized;
            Vector2 perpendicular = Vector2.Perpendicular(direction);

            Vector2Int entryPoint1 = CreateEntryPoint(room1, -direction);
            Vector2Int entryPoint2 = CreateEntryPoint(room2, direction);

            Vector2Int currentTile = entryPoint2;
            Vector2 currentPosition = entryPoint2;

            List<Vector2> points = new List<Vector2>() { entryPoint1 };
            for (int i = 1; i < INITIAL_POINTS; ++i)
            {
                Vector2 point = Vector2.Lerp(entryPoint1, entryPoint2, i / (float)INITIAL_POINTS) + perpendicular * (float)m_random.GetRandomNumber(-5.0f, 5.0f);
                points.Add(point);
            }
            points.Add(entryPoint2);
            points = m_curveGenerator.GenerateSmoothCurve(points, 3);

            for (int i = 0; i < points.Count - 1; ++i)
            {
                HashSet<Vector2Int> tiles = GenerateTilesBetweenPoints(points[i], points[i + 1]);
                generatedTiles.UnionWith(tiles);
            }

            return generatedTiles;
        }

        private Vector2Int CreateEntryPoint(in RoomData roomData, in Vector2 direction)
        {
            Vector2Int currentTile = roomData.WorldPosition;
            Vector2Int lastVisitedTile = currentTile;
            Vector2 currentPosition = roomData.WorldPosition;

            while (currentPosition.x > roomData.LeftMostTile.x && currentPosition.x < roomData.RightMostTile.x &&
                currentPosition.y > roomData.BottomMostTile.y && currentPosition.y < roomData.TopMostTile.y)
            {
                if (roomData.ContainsTile(currentTile))
                    lastVisitedTile = currentTile;

                currentPosition += direction;
                currentTile = new Vector2Int((int)currentPosition.x, (int)currentPosition.y);
            }

            currentPosition = (Vector2)lastVisitedTile - direction * OFFSET;
            return new Vector2Int((int)currentPosition.x, (int)currentPosition.y);
        }

        private HashSet<Vector2Int> GenerateTilesBetweenPoints(in Vector2 from, in Vector2 to)
        {
            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();

            Vector2Int currentTile = new Vector2Int((int)from.x, (int)from.y);
            Vector2 currentPosition = from;
            Vector2 directionToMove = to - from;

            float currentMagnitude = directionToMove.magnitude;
            float previousMagnitude = currentMagnitude;

            while (currentMagnitude <= previousMagnitude)
            {
                foreach (Vector2Int localPos in m_tilesToGrab)
                    generatedTiles.Add(currentTile + localPos);

                currentPosition += directionToMove.normalized;
                currentTile = new Vector2Int((int)currentPosition.x, (int)currentPosition.y);

                previousMagnitude = currentMagnitude;
                currentMagnitude = (to - currentPosition).magnitude;
            }

            return generatedTiles;
        }
    }
}