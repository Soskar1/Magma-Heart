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
        [SerializeField] private TileBase m_combatTile;
        private RoomGrid m_roomGrid;
        private RoomTile m_currentTile;

        public void Initialize(RoomGrid roomGrid) => m_roomGrid = roomGrid;

        private Vector3Int GetCombatTilePosition(RoomTile roomTile)
        {
            Vector2 worldPosition = m_roomGrid.TilePositionToWorld(roomTile.Position);
            return m_combatTilemap.WorldToCell(worldPosition);
        }

        public void Show(RoomTile tile)
        {
            m_currentTile = tile;
            Vector3Int combatTilePosition = GetCombatTilePosition(tile);
            m_combatTilemap.SetTile(combatTilePosition, m_combatTile);
        }

        public void Hide(RoomTile tile)
        {
            Vector3Int combatTilePosition = GetCombatTilePosition(tile);
            m_combatTilemap.SetTile(combatTilePosition, null);
        }

        public void Clear()
        {
            if (m_currentTile != null)
                Hide(m_currentTile);
        }
    }
}