using MagmaHeart.Extensions;
using System.Linq;
using UnityEngine;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI;
using System.Collections.Generic;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon.Data;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board
    {
        private readonly Dictionary<EntityModel, Entity> m_entities;
        private readonly RoomGrid m_grid;
        
        public RoomModel RoomModel { get; init; }
        public RoomData RoomData { get; init; }

        public IEnumerable<EntityModel> Models => m_entities.Keys;
        public IEnumerable<Entity> Entities => m_entities.Values;

        public Room(RoomModel model, RoomData roomData, RoomGrid grid) : base(BoardGraphBuilder.GenerateBoardGraph(model))
        {
            RoomModel = model;
            RoomData = roomData;
            m_grid = grid;
            m_entities = new Dictionary<EntityModel, Entity>();
        }

        public void AddEntityToInspect(Entity entity)
        {
            Vector2 position = entity.Model.GetCurrentTilePosition().ToVector2();

            AddUnit(position, entity.Model);
            m_entities.Add(entity.Model, entity);

            ChangeNodeType(position, BoardNodeType.Obstacle);
        }

        public void RemoveEntityFromRoom(Entity entity)
        {
            Vector2 position = entity.Model.GetCurrentTilePosition().ToVector2();

            RemoveUnit(position, entity.Model);
            m_entities.Remove(entity.Model);

            ChangeNodeType(position, BoardNodeType.Walkable);
        }

        public RoomTile GetRoomTile(Vector3 worldPosition)
        {
            Vector3Int position = m_grid.WorldToTilePosition(worldPosition);
            return new RoomTile(position, m_grid);
        }

        public bool TileIsAccessable(RoomTile roomTile)
        {
            DungeonTile tile = RoomModel.GetTile((Vector2Int)roomTile.Position);

            if (tile == null || tile.Type == TileType.Wall || EntityIsOnTile(roomTile, out _))
                return false;

            return true;
        }

        public bool TryGetUnit(Vector2 position, out EntityModel entity)
        {
            if (!TryGetUnits(position, out HashSet<AIUnitModel> units))
            {
                entity = null;
                return false;
            }

            entity = (EntityModel)units.First();
            return true;
        }

        public bool EntityIsOnTile(RoomTile roomTile, out EntityModel unit) => TryGetUnit(roomTile.Position.ToVector2(), out unit);

        public bool TryGetEntity(EntityModel model, out Entity presenter) => m_entities.TryGetValue(model, out presenter);

        public bool TryGetEntity(RoomTile roomTile, out Entity entity)
        {
            entity = null;

            if (!EntityIsOnTile(roomTile, out EntityModel model))
                return false;

            if (!TryGetEntity(model, out entity))
                return false;

            return true;
        }
    }
}