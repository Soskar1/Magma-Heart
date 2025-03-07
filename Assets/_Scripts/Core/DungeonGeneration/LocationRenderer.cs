using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer
    {
        private readonly Tilemap m_tilemap;
        private readonly TileBase m_floorTile;
        private readonly TileBase m_wallTile;

        public LocationRenderer(in Tilemap tilemap, in TileBase floorTile, in TileBase wallTile)
        {
            m_tilemap = tilemap;
            m_floorTile = floorTile;
            m_wallTile = wallTile;
        }

        public IEnumerator DrawTiles(HashSet<Vector2Int>[] rooms)
        {
            foreach (HashSet<Vector2Int> room in rooms)
            {
                foreach (Vector2Int tile in room)
                {
                    Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile);

                    if (!room.Contains(tile + Vector2Int.up) || !room.Contains(tile + Vector2Int.right) ||
                        !room.Contains(tile + Vector2Int.down) || !room.Contains(tile + Vector2Int.left) ||
                        !room.Contains(tile + new Vector2Int(1, 1)) || !room.Contains(tile + new Vector2Int(1, -1)) ||
                        !room.Contains(tile + new Vector2Int(-1, -1)) || !room.Contains(tile + new Vector2Int(-1, 1))) {
                        m_tilemap.SetTile(tilePosition, m_wallTile);
                    }
                    else
                    {
                        m_tilemap.SetTile(tilePosition, m_floorTile);
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void Clear()
        {
            m_tilemap.ClearAllTiles();
        }
    }
}