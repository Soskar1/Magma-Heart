using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(Grid))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private TileBase m_combatTile;
        public RoomTileData RoomTileData { get; private set; }
        public GameGrid Grid { get; private set; }

        private Tilemap m_combatTilemap;

        public void Initialize(RoomTileData roomTileData, GameGrid gameGrid)
        {
            RoomTileData = roomTileData;
            Grid = gameGrid;
            m_combatTilemap = GetComponentInChildren<Tilemap>();
        }

        public void TryDisplayCombatTile(Vector3Int roomTilePosition)
        {
            Vector2 worldPosition = Grid.TilePositionToWorld(roomTilePosition);
            Vector3Int tilePosition = Grid.WorldToTilePosition(worldPosition);

            if (TileIsAccessable(tilePosition))
            {
                tilePosition = Grid.WorldToTilePosition(worldPosition);
                tilePosition = m_combatTilemap.WorldToCell(worldPosition);
                m_combatTilemap.SetTile(tilePosition, m_combatTile);
            }
        }

        public void HideCombatTileAt(Vector3Int roomTilePosition)
        {
            // Here we need to convert roomTile to combatTile position, because of the offset issues
            Vector2 worldPosition = Grid.TilePositionToWorld(roomTilePosition);
            Vector3Int tilePosition = m_combatTilemap.WorldToCell(worldPosition);
            m_combatTilemap.SetTile(tilePosition, null);
        }

        public bool TileIsAccessable(Vector3Int tilePosition)
        {
            DungeonTile tile = RoomTileData.GetTile((Vector2Int)tilePosition);

            if (tile == null || tile.Type == TileType.Wall)
                return false;

            return true;
        }
    }
}