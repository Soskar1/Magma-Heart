using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap m_floors;
        [SerializeField] private Tilemap m_walls;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;
        [SerializeField] private int m_tilesPerFrame = 256;

        public Action RenderedAllTiles;

        public IEnumerator DrawTiles(HashSet<Vector2Int> tiles)
        {
            int renderedTiles = 0;
            foreach (Vector2Int tile in tiles)
            {
                Vector3Int tilePosition = m_floors.WorldToCell((Vector3Int)tile);

                if (!tiles.Contains(tile + Vector2Int.up) || !tiles.Contains(tile + Vector2Int.right) ||
                    !tiles.Contains(tile + Vector2Int.down) || !tiles.Contains(tile + Vector2Int.left) ||
                    !tiles.Contains(tile + new Vector2Int(1, 1)) || !tiles.Contains(tile + new Vector2Int(1, -1)) ||
                    !tiles.Contains(tile + new Vector2Int(-1, -1)) || !tiles.Contains(tile + new Vector2Int(-1, 1))) {
                    m_walls.SetTile(tilePosition, m_wallTile);
                }
                else
                {
                    m_floors.SetTile(tilePosition, m_floorTile);
                }

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            RenderedAllTiles?.Invoke();
        }

        public void Clear()
        {
            m_floors.ClearAllTiles();
        }
    }
}