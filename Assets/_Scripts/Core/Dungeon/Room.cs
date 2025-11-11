using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board
    {
        private CombatTilemapRenderer m_renderer;
        public RoomTileData RoomTileData { get; init; }
        public DungeonGrid Grid { get; init; }
        public Tilemap CombatTilemap => m_renderer.CombatTilemap;

        public Room(RoomTileData roomTileData, DungeonGrid gameGrid, CombatTilemapRenderer renderer, BoardGraph boardGraph) : base(boardGraph)
        {
            RoomTileData = roomTileData;
            Grid = gameGrid;
            m_renderer = renderer;
        }

        public void AddEntityToInspect(Entity entity)
        {
            Vector2 position = entity.Model.GetCurrentTilePosition().ToVector2();

            Units.Add(position, entity.Model);
            entity.TurnBasedMovement.OnMovementEnded += HandleOnMovementEnded;

            ChangeNodeType(position, BoardNodeType.Obstacle);
        }

        public void RemoveEntityFromRoom(Entity entity)
        {
            Vector2 position = entity.Model.GetCurrentTilePosition().ToVector2();

            entity.TurnBasedMovement.OnMovementEnded -= HandleOnMovementEnded;
            Units.Remove(position);

            ChangeNodeType(position, BoardNodeType.Walkable);
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

        public bool EntityIsOnTile(RoomTile roomTile, out EntityModel unit)
        {
            Vector2 position = Units.Keys.FirstOrDefault(pos => pos == roomTile.Position.ToVector2());

            if (Units.TryGetValue(position, out AIUnit aiUnit))
            {
                unit = (EntityModel)aiUnit;
                return true;
            }

            unit = null;
            return false;
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            Vector2 from = e.From.ToVector2();
            Vector2 to = e.To.ToVector2();

            AIUnit unit = Units[from];
            Units.Remove(from);
            Units.Add(to, unit);

            ChangeNodeType(from, BoardNodeType.Walkable);
            ChangeNodeType(to, BoardNodeType.Obstacle);
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