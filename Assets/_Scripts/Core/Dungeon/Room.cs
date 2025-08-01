using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(Grid))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private TileBase m_combatTile;
        public RoomTileData RoomTileData { get; private set; }

        private Tilemap m_tilemap;
        private TilemapRenderer m_tilemapRenderer;

        public void Initialize(RoomTileData roomTileData)
        {
            RoomTileData = roomTileData;
            m_tilemap = GetComponentInChildren<Tilemap>();
            m_tilemapRenderer = GetComponentInChildren<TilemapRenderer>();

            HashSet<DungeonTile> dungeonTiles = RoomTileData.GetTiles();
            foreach (DungeonTile tile in dungeonTiles)
            {
                if (tile.Type != TileType.Wall)
                {
                    Vector3Int tilePosition = m_tilemap.WorldToCell(tile.Position.ToVector3Int());
                    m_tilemap.SetTile(tilePosition, m_combatTile);
                }
            }
        }

        public void ShowCombatTiles() => m_tilemapRenderer.enabled = true;

        public void HideCombatTiles() => m_tilemapRenderer.enabled = false;
    }
}