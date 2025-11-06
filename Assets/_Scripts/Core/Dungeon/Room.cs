using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Boards;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board
    {
        private CombatTilemapRenderer m_renderer;
        public RoomTileData RoomTileData { get; init; }
        public DungeonGrid Grid { get; init; }
        public Tilemap CombatTilemap => m_renderer.CombatTilemap;

        private List<Entity> m_entitiesInCombat;

        public Room(RoomTileData roomTileData, DungeonGrid gameGrid, CombatTilemapRenderer renderer, BoardGraph boardGraph) : base(boardGraph)
        {
            RoomTileData = roomTileData;
            Grid = gameGrid;
            m_renderer = renderer;
            m_entitiesInCombat = new List<Entity>();
        }

        public void AddEntityToInspect(Entity entity)
        {
            m_entitiesInCombat.Add(entity);
            entity.TurnBasedMovement.OnMovementEnded += HandleOnMovementEnded;

            ChangeNodeType(entity.CurrentTilePosition.ToVector2(), BoardNodeType.Obstacle);
        }

        public void RemoveEntityFromRoom(Entity entity)
        {
            entity.TurnBasedMovement.OnMovementEnded -= HandleOnMovementEnded;
            m_entitiesInCombat.Remove(entity);

            ChangeNodeType(entity.CurrentTilePosition.ToVector2(), BoardNodeType.Walkable);
        }

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

        public bool EntityIsOnTile(RoomTile roomTile, out Entity entity)
        {
            entity = m_entitiesInCombat.FirstOrDefault(e => e.CurrentTilePosition == roomTile.Position);
            if (entity == null)
                return false;

            return true;
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            ChangeNodeType(e.From.ToVector2(), BoardNodeType.Walkable);
            ChangeNodeType(e.To.ToVector2(), BoardNodeType.Obstacle);
        }
    }

    public class RoomTile
    {
        private Room m_room;
        public Vector3Int Position { get; private set; }
        public Vector2 TileCenter => m_room.Grid.ToTileCenter(Position.ToVector2Int());
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