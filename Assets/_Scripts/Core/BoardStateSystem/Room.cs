using MagmaHeart.Extensions;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI;
using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.BoardStateSystem.Actions;

namespace MagmaHeart.Core.BoardStateSystem
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

            AddUnit(position, entity.Model);
            entity.TurnBasedMovement.OnMovementEnded += HandleOnMovementEnded;

            ChangeNodeType(position, BoardNodeType.Obstacle);
        }

        public void RemoveEntityFromRoom(Entity entity)
        {
            Vector2 position = entity.Model.GetCurrentTilePosition().ToVector2();

            entity.TurnBasedMovement.OnMovementEnded -= HandleOnMovementEnded;
            RemoveUnit(position, entity.Model);

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

        // TODO: Move this to custom StateChangeObject
        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            //Vector2 from = e.From.ToVector2();
            //Vector2 to = e.To.ToVector2();

            //if (!TryGetUnit(from, out EntityModel unit))
            //    throw new System.Exception($"Unit at position {from} does not exist");

            //RemoveUnit(from);
            //AddUnit(to, unit);

            //ChangeNodeType(from, BoardNodeType.Walkable);
            //ChangeNodeType(to, BoardNodeType.Obstacle);

            throw new System.Exception("FIX THIS");
        }

        public bool TryGetUnit(Vector2 position, out EntityModel entity)
        {
            if (!TryGetUnits(position, out HashSet<AIUnit> units))
            {
                entity = null;
                return false;
            }

            entity = (EntityModel)units.First();
            return true;
        }

        public bool EntityIsOnTile(RoomTile roomTile, out EntityModel unit) => TryGetUnit(roomTile.Position.ToVector2(), out unit);
    }
}