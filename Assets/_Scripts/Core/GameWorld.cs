using MagmaHeart.Abilities;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core
{
    public class GameWorld : IBoardGameWorld
    {
        private readonly WorldGrid m_worldGrid;
        private readonly List<LocationData> m_locations;
        private readonly Random m_random;
        private readonly AStar m_aStar;

        private LocationData m_currentLocation;
        private Room m_currentRoom;
        private int m_currentRoomIndex;
        private DungeonGenerator m_currentGenerator;

        public event EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        public event EventHandler OnRoomChanged;
        public event EventHandler OnLocationChanged;

        public WorldGrid WorldGrid => m_worldGrid;
        public Room CurrentRoom => m_currentRoom;

        private IDictionary<int, Entity> m_worldEntities = new Dictionary<int, Entity>();

        public GameWorld(WorldGrid worldGrid, List<LocationData> locations, Random random)
        {
            m_worldGrid = worldGrid;
            m_locations = locations;
            m_random = random;
            m_aStar = new AStar(AStar.ManhattanDistance);

            SwitchLocation(m_locations.First());
        }

        public RoomModel GenerateRoom()
        {
            RoomModel roomModel = m_currentGenerator.GenerateRoom();

            if (m_currentRoom != null)
                m_currentRoom.Dispose();

            m_currentRoom = new Room(roomModel, GetRoomData(), m_worldGrid);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(m_currentRoom);
            OnRoomGenerated?.Invoke(this, args);

            return roomModel;
        }

        public void GenerateNextRoom()
        {
            if (m_currentRoomIndex >= m_currentLocation.RoomsInLocation)
                SwitchLocation(m_currentLocation); // TODO: add more locations

            ++m_currentRoomIndex;

            GenerateRoom();

            OnRoomChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SwitchLocation(LocationData location)
        {
            m_currentLocation = location;
            m_currentRoomIndex = 0;

            TextAsset configFile = ExternalResources.LoadTextAsset(m_currentLocation.RoomGenerationConfigFileName);
            DungeonGeneratorData data = DungeonGeneratorDataDeserializer.Deserialize(configFile, m_random);
            m_currentGenerator = new DungeonGenerator(data);

            OnLocationChanged?.Invoke(this, EventArgs.Empty);
        }

        private RoomData GetRoomData()
        {
            if (m_currentRoomIndex < m_currentLocation.RoomsInLocation)
                return m_currentLocation.MonsterRoom;

            return m_currentLocation.BossRoom;
        }

        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path, bool ignoreEntities = false)
        {
            var searchGraph = m_currentRoom.Graph;

            if (ignoreEntities)
            {
                searchGraph = m_currentRoom.Graph.DeepCopy();

                foreach (var entity in m_currentRoom.Entities)
                    searchGraph.ChangeNodeType(entity.Model.TilePosition.ToVector2(), BoardNodeType.Walkable);
            }

            List<Vector2> tmpPath = m_aStar.FindPath(searchGraph, from.ToVector2Int(), to.ToVector2Int());

            if (tmpPath == null || tmpPath.Count == 0 || (Vector3)tmpPath.Last() != to)
            {
                path = null;
                return false;
            }

            path = tmpPath
                .Select(point => (Vector3)m_worldGrid.ToTileCenter(point.ToVector2Int()))
                .ToList();

            return true;
        }

        public int GetDistance(int entityId1, int entityId2)
        {
            TryGetEntity(entityId1, out Entity entity1);
            TryGetEntity(entityId2, out Entity entity2);

            return Mathf.Abs(entity1.Model.TilePosition.x - entity2.Model.TilePosition.x) +
                   Mathf.Abs(entity1.Model.TilePosition.y - entity2.Model.TilePosition.y);
        }

        public Vector3 GetEntityPosition(int entityId)
        {
            TryGetEntity(entityId, out Entity entity);
            return entity.Model.TilePosition;
        }

        public bool AreEnemiesToEachOther(int executorId, int targetId)
        {
            if (executorId == targetId)
                return false;

            TryGetEntity(executorId, out Entity executor);
            TryGetEntity(targetId, out Entity target);

            return executor.Model.IsPlayer != target.Model.IsPlayer;
        }

        public void AddEntity(Entity entity)
        {
            m_worldEntities.TryAdd(entity.Model.Id, entity);
            CurrentRoom.AddEntity(entity);
        }

        public void RemoveEntity(int entityId)
        {
            m_worldEntities.Remove(entityId);
            CurrentRoom.RemoveEntity(entityId);
        }

        public bool TryGetEntity(int entityId, out Entity entity)
        {
            if (m_worldEntities.TryGetValue(entityId, out entity))
                return true;

            return m_currentRoom.TryGetEntity(entityId, out entity);
        }

        public bool TryGetEntityAtPosition(Vector3 position, out Entity entity) => m_currentRoom.TryGetEntity(position, out entity);

        public DungeonTile GetTile(Vector3 worldPosition)
        {
            if (m_currentRoom == null)
                return null;

            return m_currentRoom.GetTile(worldPosition);
        }

        public Vector2 WorldToTilePosition(Vector2 worldPosition) => m_worldGrid.WorldToTilePosition(worldPosition).ToVector2();
        public Vector2 ToTileCenter(Vector2Int tile) => m_worldGrid.ToTileCenter(tile);

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => m_currentRoom.ChangeNodeType(position, newNodeType);
        public BoardNodeType GetNodeType(Vector2 position) => m_currentRoom.GetNodeType(position);

        public void AddUnit(Vector2 position, AIUnitModel unit) => m_currentRoom.AddUnit(position, unit);
        public bool RemoveUnit(Vector2 position) => m_currentRoom.RemoveUnit(position);
        public AIUnitModel GetUnit(int id) => CurrentRoom.TryGetUnit(id, out AIUnitModel unit) ? unit : null;

        public void MoveUnit(int unitId, Vector2 newPosition)
        {
            CurrentRoom.TryGetUnit(unitId, out AIUnitModel unit);
            CurrentRoom.TryGetUnitPosition(unitId, out Vector2 oldPosition);

            CurrentRoom.RemoveUnit(oldPosition);
            CurrentRoom.AddUnit(newPosition, unit);
        }

        public IEnumerable<AIUnitModel> GetUnits() => m_currentRoom.GetUnits();

        public IParameter GetParameter(int entityId, ParameterId parameter)
        {
            TryGetEntity(entityId, out Entity entity);
            return entity.Model.GetParameter(parameter);
        }

        public void SetParameter(int entityId, ParameterId parameter, float newValue)
        {
            m_currentRoom.TryGetUnit(entityId, out AIUnitModel unit);
            unit.Parameters[parameter].SetValue(newValue);
        }

        public void SetCooldown(int unitId, string abilityId, int turns)
        {
            AIUnitModel unit = GetUnit(unitId);
            unit.SetCooldown(abilityId, turns);
        }

        public int GetCooldown(int entityId, string abilityId)
        {
            AIUnitModel unit = GetUnit(entityId);
            return unit.GetCooldown(abilityId);
        }
    }
}
