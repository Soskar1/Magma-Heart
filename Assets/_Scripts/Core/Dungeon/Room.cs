using MagmaHeart.Core.CombatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class Room
    {
        private CombatTilemapRenderer m_renderer;
        public RoomTileData RoomTileData { get; private set; }
        public DungeonGrid Grid { get; private set; }
        public Tilemap CombatTilemap => m_renderer.CombatTilemap;

        private List<ICombatController> m_entitiesInCombat;

        public Room(RoomTileData roomTileData, DungeonGrid gameGrid, CombatTilemapRenderer renderer)
        {
            RoomTileData = roomTileData;
            Grid = gameGrid;
            m_renderer = renderer;
            m_entitiesInCombat = new List<ICombatController>();
        }

        public void AddEntityToInspect(ICombatController combatController) => m_entitiesInCombat.Add(combatController);

        public RoomTile GetRoomTile(Vector3 worldPosition)
        {
            Vector3Int position = Grid.WorldToTilePosition(worldPosition);
            return new RoomTile(this, position);
        }

        public void TryDisplayCombatTile(RoomTile roomTile)
        {
            if (!TileIsAccessable(roomTile))
                return;

            CombatTile combatTile = roomTile.ToCombatTile();
            m_renderer.DisplayCombatTile(combatTile);
        }

        public void HideCombatTileAt(RoomTile roomTile)
        {
            CombatTile combatTile = roomTile.ToCombatTile();
            m_renderer.HideCombatTileAt(combatTile);
        }

        public bool TileIsAccessable(RoomTile roomTile)
        {
            DungeonTile tile = RoomTileData.GetTile((Vector2Int)roomTile.Position);

            if (tile == null || tile.Type == TileType.Wall || EntityIsOnTile(roomTile, out _))
                return false;

            return true;
        }

        public bool EntityIsOnTile(RoomTile roomTile, out IHittableTile entity)
        {
            entity = m_entitiesInCombat.FirstOrDefault(e => e.CurrentTilePosition == roomTile.Position);
            if (entity == null)
                return false;

            return true;
        }
    }

    public class RoomTile
    {
        private Room m_room;
        public Vector3Int Position { get; private set; }
        public RoomTile(Room room, Vector3Int position)
        {
            m_room = room;
            Position = position;
        }

        public CombatTile ToCombatTile()
        {
            Vector2 worldPosition = m_room.Grid.TilePositionToWorld(Position);
            Vector3Int combatTilePosition = m_room.CombatTilemap.WorldToCell(worldPosition);
            return new CombatTile(combatTilePosition);
        }
    }

    public class CombatTile
    {
        public Vector3Int Position { get; private set; }

        public CombatTile(Vector3Int position) => Position = position;
    }
}