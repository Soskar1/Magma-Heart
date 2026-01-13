using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.DungeonGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonController
    {
        public Room CurrentRoom { get; private set; }
        public RoomGrid Grid { get; init; }
        
        private readonly List<LocationData> m_locations;
        private readonly Random m_random;

        private LocationData m_currentLocation;
        private DungeonGenerator m_currentGenerator;
        private int m_currentRoomIndex;

        public event EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        public event EventHandler OnRoomChanged;
        
        public DungeonController(RoomGrid roomGrid, List<LocationData> locations, Random random)
        {
            Grid = roomGrid;
            m_locations = locations;
            m_random = random;

            SwitchLocation(m_locations.First());
        }

        public RoomModel GenerateRoom()
        {
            RoomModel roomModel = m_currentGenerator.GenerateRoom();
            CurrentRoom = new Room(roomModel, GetRoomData(), Grid);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(CurrentRoom);
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
        }

        private RoomData GetRoomData()
        {
            if (m_currentRoomIndex < m_currentLocation.RoomsInLocation)
                return m_currentLocation.MonsterRoom;

            return m_currentLocation.BossRoom;
        }
    }
}
