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

        public IEnumerator DrawTiles(HashSet<RoomData> roomDatas)
        {
            foreach (RoomData roomData in roomDatas)
            {
                HashSet<Vector2Int> tiles = roomData.GetTilesCopy();

                foreach (Vector2Int tile in tiles)
                {
                    Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile);

                    if (!roomData.ContainsTile(tile + Vector2Int.up) || !roomData.ContainsTile(tile + Vector2Int.right) ||
                        !roomData.ContainsTile(tile + Vector2Int.down) || !roomData.ContainsTile(tile + Vector2Int.left) ||
                        !roomData.ContainsTile(tile + new Vector2Int(1, 1)) || !roomData.ContainsTile(tile + new Vector2Int(1, -1)) ||
                        !roomData.ContainsTile(tile + new Vector2Int(-1, -1)) || !roomData.ContainsTile(tile + new Vector2Int(-1, 1))) {
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