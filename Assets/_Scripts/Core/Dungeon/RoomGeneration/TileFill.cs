using System.Collections.Generic;
using System.Linq;
using MagmaHeart.DungeonGeneration;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon.RoomGeneration
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

        public void GenerateRoom(in RoomModel roomModel)
        {
            HashSet<Vector2Int> emptyTiles = new HashSet<Vector2Int>();

            for (int x = roomModel.LeftBorder; x <= roomModel.RightBorder; ++x)
                for (int y = roomModel.BottomBorder; y <= roomModel.TopBorder; ++y)
                    emptyTiles.Add(new Vector2Int(x, y));

            emptyTiles.ExceptWith(roomModel.GetTilePositions());

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

                        if (neighbourTile.x >= roomModel.LeftBorder && neighbourTile.x <= roomModel.RightBorder &&
                            neighbourTile.y >= roomModel.BottomBorder && neighbourTile.y <= roomModel.TopBorder)
                        {
                            if (!roomModel.ContainsTileAtPosition(neighbourTile) && !filledSpace.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                                tilesToVisit.Enqueue(neighbourTile);
                        }
                        else
                        {
                            touchesBorder = true;
                        }
                    }
                }

                if (!touchesBorder)
                    roomModel.AddTiles(filledSpace, TileType.Floor);

                emptyTiles.ExceptWith(filledSpace);
            }
        }
    }
}