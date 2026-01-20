using MagmaHeart.Core.BoardStateSystem.Actions.Preview;
using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.BoardStateSystem
{
    [RequireComponent(typeof(Grid))]
    public class CombatTilemapRenderer : MonoBehaviour, ICombatTileHighlighter
    {
        [SerializeField] private Tilemap m_combatTilemap;
        [SerializeField] private TileBase m_validCombatTile;
        [SerializeField] private TileBase m_invalidCombatTile;
        private RoomGrid m_roomGrid;

        public void Initialize(RoomGrid roomGrid) => m_roomGrid = roomGrid;

        private Vector3Int GetCombatTilePosition(RoomTile roomTile)
        {
            Vector2 worldPosition = m_roomGrid.TilePositionToWorld(roomTile.Position);
            return m_combatTilemap.WorldToCell(worldPosition);
        }

        public void Show(RoomTile tile, bool isValid)
        {
            if (tile == null)
                return;

            Vector3Int combatTilePosition = GetCombatTilePosition(tile);

            TileBase tileToSet = isValid ? m_validCombatTile : m_invalidCombatTile;
            m_combatTilemap.SetTile(combatTilePosition, tileToSet);
        }

        public void Hide(RoomTile tile)
        {
            if (tile == null)
                return;

            Vector3Int combatTilePosition = GetCombatTilePosition(tile);
            m_combatTilemap.SetTile(combatTilePosition, null);
        }

        public void Clear() => m_combatTilemap.ClearAllTiles();
    }
}