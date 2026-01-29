using MagmaHeart.Extensions;
using UnityEngine;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Boards;
using System.Collections.Generic;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon.Data;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board
    {
        private readonly Dictionary<int, Entity> m_entities;
        private readonly RoomGrid m_grid;
        
        public RoomModel RoomModel { get; init; }
        public RoomData RoomData { get; init; }

        public IEnumerable<Entity> Entities => m_entities.Values;

        public Room(RoomModel model, RoomData roomData, RoomGrid grid) : base(BoardGraphBuilder.GenerateBoardGraph(model))
        {
            RoomModel = model;
            RoomData = roomData;
            m_grid = grid;
            m_entities = new Dictionary<int, Entity>();
        }

        public void AddEntityToInspect(Entity entity)
        {
            Vector2 position = entity.Model.TilePosition.ToVector2();

            AddUnit(position, entity.Model);
            m_entities.Add(entity.Model.Id, entity);

            ChangeNodeType(position, BoardNodeType.Obstacle);
        }

        public void RemoveEntityFromRoom(Entity entity)
        {
            Vector2 position = entity.Model.TilePosition.ToVector2();

            RemoveUnit(position);
            m_entities.Remove(entity.Model.Id);

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

            if (tile == null || tile.Type == TileType.Wall || TryGetEntity(roomTile.Position, out _))
                return false;

            return true;
        }

        public bool TryGetEntity(int entityId, out Entity entity) => m_entities.TryGetValue(entityId, out entity);
        public bool TryGetEntity(Vector3Int position, out Entity entity)
        {
            if (!TryGetUnit(position.ToVector2(), out EntityModel model))
            {
                entity = null;
                return false;
            }

            return TryGetEntity(model.Id, out entity);
        }
    }
}