using MagmaHeart.AI.Boards;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board
    {
        private readonly Dictionary<int, Entity> m_entities;
        private readonly WorldGrid m_grid;
        
        public RoomModel RoomModel { get; init; }
        public RoomData RoomData { get; init; }

        public IEnumerable<Entity> Entities => m_entities.Values;

        public Room(RoomModel model, RoomData roomData, WorldGrid grid) : base(BoardGraphBuilder.GenerateBoardGraph(model))
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

        public bool TileIsAccessable(Vector3 worldPosition)
        {
            DungeonTile tile = GetTile(worldPosition);

            if (tile == null || tile.Type == TileType.Wall || TryGetEntity(worldPosition, out _))
                return false;

            return true;
        }

        public bool TryGetEntity(int entityId, out Entity entity) => m_entities.TryGetValue(entityId, out entity);
        public bool TryGetEntity(Vector3 position, out Entity entity)
        {
            DungeonTile tile = GetTile(position);

            if (!TryGetUnit(tile.Position, out EntityModel model))
            {
                entity = null;
                return false;
            }

            return TryGetEntity(model.Id, out entity);
        }

        public DungeonTile GetTile(Vector3 worldPosition)
        {
            Vector3Int position = m_grid.WorldToTilePosition(worldPosition);
            return RoomModel.GetTile(position.ToVector2Int());
        }
    }
}