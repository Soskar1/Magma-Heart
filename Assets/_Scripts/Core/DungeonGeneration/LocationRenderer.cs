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
        private readonly int m_tilesPerFrame = 256;

        public LocationRenderer(in Tilemap tilemap, in TileBase floorTile, in TileBase wallTile)
        {
            m_tilemap = tilemap;
            m_floorTile = floorTile;
            m_wallTile = wallTile;
        }

        public IEnumerator DrawTiles(HashSet<Vector2Int> tiles)
        {
            int renderedTiles = 0;
            foreach (Vector2Int tile in tiles)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile);

                if (!tiles.Contains(tile + Vector2Int.up) || !tiles.Contains(tile + Vector2Int.right) ||
                    !tiles.Contains(tile + Vector2Int.down) || !tiles.Contains(tile + Vector2Int.left) ||
                    !tiles.Contains(tile + new Vector2Int(1, 1)) || !tiles.Contains(tile + new Vector2Int(1, -1)) ||
                    !tiles.Contains(tile + new Vector2Int(-1, -1)) || !tiles.Contains(tile + new Vector2Int(-1, 1))) {
                    m_tilemap.SetTile(tilePosition, m_wallTile);
                }
                else
                {
                    m_tilemap.SetTile(tilePosition, m_floorTile);
                }

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }
        }

        public void Clear()
        {
            m_tilemap.ClearAllTiles();
        }
    }
}