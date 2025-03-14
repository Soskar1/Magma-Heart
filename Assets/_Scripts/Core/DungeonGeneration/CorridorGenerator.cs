using UnityEngine;
using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class CorridorGenerator
    {
        private List<Vector2Int> m_tilesToGrab;

        public CorridorGenerator(in int corridorSize)
        {
            m_tilesToGrab = new List<Vector2Int>();

            for (int x = -corridorSize; x <= corridorSize; x++)
                for (int y = -corridorSize; y <= corridorSize; y++)
                    if (x * x + y * y <= corridorSize * corridorSize)
                        m_tilesToGrab.Add(new Vector2Int(x, y));
        }

        public HashSet<Vector2Int> GenerateCorridor(in RoomData room1, in RoomData room2)
        {
            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();
            Vector2 direction = room1.WorldPosition - room2.WorldPosition;

            Vector2Int entryPoint1 = CreateEntryPoint(room1, -direction.normalized);
            Vector2Int entryPoint2 = CreateEntryPoint(room2, direction.normalized);

            Vector2Int currentTile = entryPoint2;
            Vector2 currentPosition = entryPoint2;

            while ((currentTile - entryPoint1).magnitude > 1.0f)
            {
                foreach (Vector2Int localPos in m_tilesToGrab)
                    generatedTiles.Add(currentTile + localPos);

                currentPosition += direction.normalized;
                currentTile = new Vector2Int((int)currentPosition.x, (int)currentPosition.y);
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

            return lastVisitedTile;
        }
    }
}