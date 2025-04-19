using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TileFill : IRoomGenerator
    {
        private readonly List<Vector2Int> m_directionsToVisit;

        public TileFill()
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };
        }

        public void GenerateRoom(in RoomTileData roomTileData)
        {
            HashSet<Vector2Int> emptyTiles = new HashSet<Vector2Int>();

            for (int x = roomTileData.LeftBorder; x <= roomTileData.RightBorder; ++x)
                for (int y = roomTileData.BottomBorder; y <= roomTileData.TopBorder; ++y)
                    emptyTiles.Add(new Vector2Int(x, y));

            emptyTiles.ExceptWith(roomTileData.GetTilePositions());

            while (emptyTiles.Count > 0)
            {
                Vector2Int start = emptyTiles.First();
                bool touchesBorder = false;

                HashSet<Vector2Int> filledSpace = new HashSet<Vector2Int>();
                Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
                tilesToVisit.Enqueue(start);

                while (tilesToVisit.Count > 0)
                {
                    Vector2Int tile = tilesToVisit.Dequeue();
                    filledSpace.Add(tile);

                    foreach (Vector2Int direction in m_directionsToVisit)
                    {
                        Vector2Int neighbourTile = tile + direction;

                        if (neighbourTile.x >= roomTileData.LeftBorder && neighbourTile.x <= roomTileData.RightBorder &&
                            neighbourTile.y >= roomTileData.BottomBorder && neighbourTile.y <= roomTileData.TopBorder)
                        {
                            if (!roomTileData.ContainsTileAtPosition(neighbourTile) && !filledSpace.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                                tilesToVisit.Enqueue(neighbourTile);
                        }
                        else
                        {
                            touchesBorder = true;
                        }
                    }
                }

                if (!touchesBorder)
                    roomTileData.AddTiles(filledSpace, TileType.Floor);

                emptyTiles.ExceptWith(filledSpace);
            }
        }
    }
}