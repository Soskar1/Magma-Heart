using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonController
    {
        public Room CurrentRoom { get; private set; }
        public RoomGrid Grid { get; init; }

        // TODO: We will need to move this parameter to the room data class
        public List<EntityData> EnemyPool { get; init; }

        private DungeonGenerator m_generator;

        public EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        
        public DungeonController(DungeonGenerator generator, RoomGrid roomGrid, List<EntityData> enemyPool)
        {
            m_generator = generator;
            Grid = roomGrid;
            EnemyPool = enemyPool;
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
