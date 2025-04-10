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

        public void GenerateRoom(in RoomTileData RoomTileData)
        {
            HashSet<Vector2Int> emptyTiles = new HashSet<Vector2Int>();

            for (int x = RoomTileData.LeftBorder; x <= RoomTileData.RightBorder; ++x)
                for (int y = RoomTileData.BottomBorder; y <= RoomTileData.TopBorder; ++y)
                    emptyTiles.Add(new Vector2Int(x, y));

            emptyTiles.ExceptWith(RoomTileData.GetTilePositions());

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

                        if (neighbourTile.x >= RoomTileData.LeftBorder && neighbourTile.x <= RoomTileData.RightBorder &&
                            neighbourTile.y >= RoomTileData.BottomBorder && neighbourTile.y <= RoomTileData.TopBorder)
                        {
                            if (!RoomTileData.ContainsTileAtPosition(neighbourTile) && !filledSpace.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                                tilesToVisit.Enqueue(neighbourTile);
                        }
                        else
                        {
                            touchesBorder = true;
                        }
                    }
                }

                if (!touchesBorder)
                    RoomTileData.AddTiles(filledSpace, TileType.Floor);

                emptyTiles.ExceptWith(filledSpace);
            }
        }
    }
}