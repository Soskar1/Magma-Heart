using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
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
    public class GameWorld : IGameWorld
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

        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path)
        {
            List<Vector2> tmpPath = m_aStar.FindPath(m_currentRoom.Graph, from.ToVector2Int(), to.ToVector2Int());

            if (tmpPath == null || tmpPath.Count == 0 || (Vector3)tmpPath.Last() != to)
            {
                path = null;
                return false;
            }

            path = tmpPath.Cast<Vector3>().ToList();
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

        public float GetResource(int entityId, ResourceId resource)
        {
            TryGetEntity(entityId, out Entity entity);

            if (resource.Id == entity.Energy.ResourceId.Id)
                return entity.Energy.CurrentEnergy;

            return 0;
        }

        public bool AreEnemiesToEachOther(int executorId, int targetId)
        {
            TryGetEntity(executorId, out Entity executor);
            TryGetEntity(targetId, out Entity target);

            return executor.Model.IsPlayer != target.Model.IsPlayer;
        }

        public int GetEntityAtPosition(Vector3 position)
        {
            if (TryGetEntityAtPosition(position, out Entity entity))
                return entity.Model.Id;

            return -1;
        }

        public bool PositionIsAccessible(Vector3 position) => m_currentRoom.TileIsAccessable(position);

        public bool TryGetEntity(int entityId, out Entity entity) => m_currentRoom.TryGetEntity(entityId, out entity);
        public bool TryGetEntityAtPosition(Vector3 position, out Entity entity) => m_currentRoom.TryGetEntity(position, out entity);

        public DungeonTile GetTile(Vector3 worldPosition)
        {
            if (m_currentRoom == null)
                return null;

            return m_currentRoom.GetTile(worldPosition);
        }

        public Vector3Int WorldToTilePosition(Vector2 worldPosition) => m_worldGrid.WorldToTilePosition(worldPosition);
        public Vector2 ToTileCenter(Vector2Int tile) => m_worldGrid.ToTileCenter(tile);
    }
}
