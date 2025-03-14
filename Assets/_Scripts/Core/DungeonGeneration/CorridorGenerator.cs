using UnityEngine;
using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class CorridorGenerator
    {
        public HashSet<Vector2Int> GenerateCorridor(in RoomData room1, in RoomData room2)
        {
            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();
            Vector2 direction = room1.WorldPosition - room2.WorldPosition;

            Vector2Int entryPoint1 = CreateEntryPoint(room1, -direction.normalized);
            Vector2Int entryPoint2 = CreateEntryPoint(room2, direction.normalized);

            Vector2Int currentTile = entryPoint2;
            Vector2 currentPosition = entryPoint2;

            while ((currentTile - entryPoint1).magnitude > 1.25f)
            {
                generatedTiles.Add(currentTile);
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