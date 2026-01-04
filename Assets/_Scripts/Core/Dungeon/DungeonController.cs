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

        public EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        
        public DungeonController(RoomGrid roomGrid, List<LocationData> locations, Random random)
        {
            Grid = roomGrid;
            m_locations = locations;
            m_random = random;

            SwitchLocation(m_locations.First());
        }

        public void GetNextRoom()
        {
            RoomModel roomModel = m_currentGenerator.GenerateRoom();
            CurrentRoom = new Room(roomModel, Grid);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(CurrentRoom);
            OnRoomGenerated?.Invoke(this, args);
        }

        private void SwitchLocation(LocationData location)
        {
            m_currentLocation = location;

            TextAsset configFile = ExternalResources.LoadTextAsset(m_currentLocation.RoomGenerationConfigFileName);
            DungeonGeneratorData data = DungeonGeneratorDataDeserializer.Deserialize(configFile, m_random);
            m_currentGenerator = new DungeonGenerator(data);
        }
    }
}
