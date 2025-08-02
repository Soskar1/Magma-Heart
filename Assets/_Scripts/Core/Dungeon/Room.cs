using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(Grid))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private TileBase m_combatTile;
        public RoomTileData RoomTileData { get; private set; }

        private Tilemap m_combatTilemap;
        private GameGrid m_grid;

        private Vector3Int? m_previousDisplayedTile;

        public void Initialize(RoomTileData roomTileData, GameGrid gameGrid)
        {
            RoomTileData = roomTileData;
            m_grid = gameGrid;
            m_combatTilemap = GetComponentInChildren<Tilemap>();
        }

        public void TryDisplayCombatTile(Vector2 worldPosition)
        {
            Vector3Int tilePosition = m_grid.WorldToTilePosition(worldPosition);
            DungeonTile tile = RoomTileData.GetTile((Vector2Int)tilePosition);

            if (tile == null)
            {
                TryHidePreviousCombatTile();
            }
            else
            {
                if (tile.Type != TileType.Wall)
                {
                    TryHidePreviousCombatTile();

                    // Need to use combat tilemap for getting a new position
                    // Can't use previously defined tilePosition, because of the offset issues
                    tilePosition = m_combatTilemap.WorldToCell(worldPosition);
                    m_combatTilemap.SetTile(tilePosition, m_combatTile);
                    m_previousDisplayedTile = tilePosition;
                }
                else
                {
                    TryHidePreviousCombatTile();
                }
            }
        }

        public void TryHidePreviousCombatTile()
        {
            if (m_previousDisplayedTile.HasValue)
                m_combatTilemap.SetTile(m_previousDisplayedTile.Value, null);
        }
    }
}