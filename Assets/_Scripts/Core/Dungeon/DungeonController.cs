using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.DungeonGeneration;
using System;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonController
    {
        public Room Room { get; private set; }
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

        public async Task GenerateRoom()
        {
            RoomModel roomModel = m_generator.GenerateRoom();
            Room = new Room(roomModel, m_grid, m_tilemapRenderer);

            OnRoomGeneratedEventArgs args = new OnRoomGeneratedEventArgs(Room);
            OnRoomGenerated?.Invoke(this, args);
        }
    }
}
