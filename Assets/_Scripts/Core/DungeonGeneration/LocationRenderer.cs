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

        public IEnumerator DrawTiles(HashSet<DungeonTile> tiles)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in tiles)
            {
                Vector3Int tilePosition = m_floors.WorldToCell((Vector3Int)tile.Position);

                if (tile.TileType == TileType.Wall)
                    m_walls.SetTile(tilePosition, m_wallTile);
                else
                    m_floors.SetTile(tilePosition, m_floorTile);

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            RenderedAllTiles?.Invoke();
        }

        public void Clear()
        {
            m_floors.ClearAllTiles();
            m_walls.ClearAllTiles();
        }
    }
}