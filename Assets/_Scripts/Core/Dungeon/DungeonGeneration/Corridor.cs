using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Corridor
    {
        private RoomTileData m_room1;
        private RoomTileData m_room2;

        public RoomTileData Room1 => m_room1;
        public RoomTileData Room2 => m_room2;

        private HashSet<DungeonTile> m_tiles = new HashSet<DungeonTile>();
        public HashSet<DungeonTile> Tiles => m_tiles;

        private HashSet<DungeonTile> m_entranceTiles = new HashSet<DungeonTile>();
        public HashSet<DungeonTile> EntranceTiles => m_entranceTiles;

        public Corridor(RoomTileData room1, RoomTileData room2)
        {
            m_room1 = room1;
            m_room2 = room2;
        }

        public void AddTile(DungeonTile tile) => m_tiles.Add(tile);
        public void AddEntranceTile(DungeonTile tile) => m_entranceTiles.Add(tile);

        public void Close()
        {
            foreach (DungeonTile tile in m_entranceTiles)
                if (tile.Type == TileType.Floor)
                    tile.Type = TileType.Wall;
        }

        public void Open()
        {
            foreach (DungeonTile tile in m_entranceTiles)
                if (tile.Type == TileType.Wall)
                    tile.Type = TileType.Floor;
        }
    }
}