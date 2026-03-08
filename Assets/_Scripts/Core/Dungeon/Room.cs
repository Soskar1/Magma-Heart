using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : Board, IDisposable
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

        public void AddEntity(Entity entity)
        {
            Vector2 position = entity.Model.TilePosition.ToVector2();

            AddUnit(position, entity.Model);
            m_entities.Add(entity.Model.Id, entity);

            ChangeNodeType(position, BoardNodeType.Obstacle);
        }

        public void RemoveEntity(int entityId)
        {
            Entity entity = m_entities[entityId];
            m_entities.Remove(entityId);

            Vector2 position = entity.Model.TilePosition.ToVector2();
            RemoveUnit(position);

            ChangeNodeType(position, BoardNodeType.Walkable);
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

        public void Dispose()
        {
            IReadOnlyList<int> leftEntities = m_entities.Keys.ToList();
            foreach (var entityId in leftEntities)
                RemoveEntity(entityId);

            m_entities.Clear();
        }
    }
}