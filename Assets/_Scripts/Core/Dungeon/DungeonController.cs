using MagmaHeart.DungeonGeneration;
using System;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonController
    {
        public Room CurrentRoom { get; private set; }
        public RoomGrid Grid { get; init; }
        private DungeonGenerator m_generator;

        public EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        
        public DungeonController(DungeonGenerator generator, RoomGrid roomGrid)
        {
            m_generator = generator;
            Grid = roomGrid;
        }

        public void GenerateRoom()
        {
            RoomModel roomModel = m_generator.GenerateRoom();
            CurrentRoom = new Room(roomModel, Grid);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(CurrentRoom);
            OnRoomGenerated?.Invoke(this, args);
        }
    }
}
