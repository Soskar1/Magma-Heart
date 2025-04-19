using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Corridor
    {
        public TileData TileData { get; private set; }
        private RoomTileData m_room1;
        private RoomTileData m_room2;

        public RoomTileData Room1 => m_room1;
        public RoomTileData Room2 => m_room2;

        public HashSet<DungeonTile> BlockingTiles { get; private set; }

        public Corridor(RoomTileData room1, RoomTileData room2)
        {
            m_room1 = room1;
            m_room2 = room2;
            TileData = new TileData();
            BlockingTiles = new HashSet<DungeonTile>();
        }
    }
}