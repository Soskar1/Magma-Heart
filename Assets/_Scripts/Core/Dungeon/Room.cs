using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(Grid))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private TileBase m_combatTile;
        public RoomTileData RoomTileData { get; private set; }
        public DungeonGrid Grid { get; private set; }

        private Tilemap m_combatTilemap;

        public void Initialize(RoomTileData roomTileData, DungeonGrid gameGrid)
        {
            RoomTileData = roomTileData;
            Grid = gameGrid;
            m_combatTilemap = GetComponentInChildren<Tilemap>();
        }

        public void TryDisplayCombatTile(Vector3Int roomTilePosition)
        {
            if (!TileIsAccessable(roomTilePosition))
                return;
            
            Vector3Int combatTilePosition = ConvertToCombatTilemap(roomTilePosition);
            m_combatTilemap.SetTile(combatTilePosition, m_combatTile);
        }

        public void HideCombatTileAt(Vector3Int roomTilePosition)
        {
            Vector3Int combatTilePosition = ConvertToCombatTilemap(roomTilePosition);
            m_combatTilemap.SetTile(combatTilePosition, null);
        }

        public bool TileIsAccessable(Vector3Int tilePosition)
        {
            DungeonTile tile = RoomTileData.GetTile((Vector2Int)tilePosition);

            if (tile == null || tile.Type == TileType.Wall)
                return false;

            return true;
        }

        // Convertation from roomTile to combatTile position is necessary, because of the offset issues
        private Vector3Int ConvertToCombatTilemap(Vector3Int roomTilePosition)
        {
            Vector2 worldPosition = Grid.TilePositionToWorld(roomTilePosition);
            return m_combatTilemap.WorldToCell(worldPosition);
        }    
    }
}