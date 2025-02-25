using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TileFill : IRoomModifier
    {
        private readonly List<Vector2Int> m_directionsToVisit;
        private readonly HashSet<Vector2Int> m_region;

        public TileFill(RoomData roomData)
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            m_region = new HashSet<Vector2Int>();

            for (int x = roomData.LeftBorder; x <= roomData.RightBorder; ++x)
                for (int y = roomData.BottomBorder; y <= roomData.UpperBorder; ++y)
                    m_region.Add(new Vector2Int(x, y));
        }

        public HashSet<Vector2Int> ModifyRoom(in HashSet<Vector2Int> tiles)
        {
            if (tiles == null)
            {
                Debug.LogWarning("tiles is null. Returning new empty HashSet object");
                return new HashSet<Vector2Int>();
            }

            if (tiles.Count == 0)
            {
                Debug.LogWarning("tiles is empty. Terminating job");
                return tiles;
            }

            HashSet<Vector2Int> emptyTiles = new HashSet<Vector2Int>(m_region);
            emptyTiles.ExceptWith(tiles);

            while (emptyTiles.Count > 0)
            {
                Vector2Int start = emptyTiles.First();
                bool touchesBorder = false;

                HashSet<Vector2Int> emptySpace = new HashSet<Vector2Int>();
                Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
                tilesToVisit.Enqueue(start);

                while (tilesToVisit.Count > 0)
                {
                    Vector2Int tile = tilesToVisit.Dequeue();
                    emptySpace.Add(tile);

                    foreach (Vector2Int direction in m_directionsToVisit)
                    {
                        Vector2Int neighbourTile = tile + direction;

                        if (m_region.Contains(neighbourTile))
                        {
                            if (!tiles.Contains(neighbourTile) && !emptySpace.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                                tilesToVisit.Enqueue(neighbourTile);
                        }
                        else
                        {
                            touchesBorder = true;
                        }
                    }
                }

                if (!touchesBorder)
                    tiles.UnionWith(emptySpace);

                emptyTiles.ExceptWith(emptySpace);
            }

            return tiles;
        }
    }
}