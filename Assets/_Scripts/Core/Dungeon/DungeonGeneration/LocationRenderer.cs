using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer : MonoBehaviour
    {
        [SerializeField] private int m_tilesPerFrame = 256;
        private HashSet<Tilemap> m_usedTilemaps = new HashSet<Tilemap>();

        public Action RenderedAllTiles;

        public IEnumerator DrawTiles(HashSet<DungeonTile> tiles, Tilemap tilemap, TileBase tileBase)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in tiles)
            {
                Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int)tile.Position);
                tilemap.SetTile(tilePosition, tileBase);

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            m_usedTilemaps.Add(tilemap);
            RenderedAllTiles?.Invoke();
        }

        public void Clear()
        {
            foreach (Tilemap tilemap in m_usedTilemaps)
                tilemap.ClearAllTiles();
        }
    }
}