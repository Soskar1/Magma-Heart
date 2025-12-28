using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.DungeonGeneration;
using System;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonController
    {
        public Room CurrentRoom { get; private set; }
        private DungeonGenerator m_generator;
        private RoomGrid m_grid;
        private CombatTilemapRenderer m_tilemapRenderer;

        public EventHandler<OnRoomGeneratedEventArgs> OnRoomGenerated;
        
        public DungeonController(DungeonGenerator generator, RoomGrid roomGrid, CombatTilemapRenderer combatTilemapRenderer)
        {
            m_generator = generator;
            m_grid = roomGrid;
            m_tilemapRenderer = combatTilemapRenderer;
        }

        public void GenerateRoom()
        {
            RoomModel roomModel = m_generator.GenerateRoom();
            CurrentRoom = new Room(roomModel, m_grid, m_tilemapRenderer);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(CurrentRoom);
            OnRoomGenerated?.Invoke(this, args);
        }
    }
}
