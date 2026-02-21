using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core
{
    public class GameWorld : IGameWorld
    {
        private readonly RoomGrid m_roomGrid;
        private readonly List<LocationData> m_locations;
        private readonly Random m_random;

        private LocationData m_currentLocation;
        private Room m_currentRoom;
        private int m_currentRoomIndex;
        private DungeonGenerator m_currentGenerator;

        public event EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        public event EventHandler OnRoomChanged;
        public event EventHandler OnLocationChanged;

        public GameWorld(RoomGrid roomGrid, List<LocationData> locations, Random random)
        {
            m_roomGrid = roomGrid;
            m_locations = locations;
            m_random = random;

            SwitchLocation(m_locations.First());
        }

        public RoomModel GenerateRoom()
        {
            RoomModel roomModel = m_currentGenerator.GenerateRoom();
            m_currentRoom = new Room(roomModel, GetRoomData(), m_roomGrid);

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

        public List<Vector3Int> FindPath(Vector3Int from, Vector3Int to)
        {
            throw new System.NotImplementedException();
        }

        public int GetDistance(int entityId1, int entityId2)
        {
            m_currentRoom.TryGetEntity(entityId1, out Entity entity1);
            m_currentRoom.TryGetEntity(entityId2, out Entity entity2);

            return Mathf.Abs(entity1.Model.TilePosition.x - entity2.Model.TilePosition.x) +
                   Mathf.Abs(entity1.Model.TilePosition.y - entity2.Model.TilePosition.y);
        }

        public Vector3Int GetPosition(int entityId)
        {
            m_currentRoom.TryGetEntity(entityId, out Entity entity);
            return entity.Model.TilePosition;
        }

        public float GetResource(int entityId, ResourceId resource)
        {
            m_currentRoom.TryGetEntity(entityId, out Entity entity);
            return entity.Model.GetResource(resource);
        }

        public bool IsEnemy(int executorId, int targetId)
        {
            m_currentRoom.TryGetEntity(executorId, out Entity executor);
            m_currentRoom.TryGetEntity(targetId, out Entity target);

            return executor.Model.IsPlayer != target.Model.IsPlayer;
        }
    }
}
