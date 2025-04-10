using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomData> Rooms { get; private set; }
        public HashSet<DungeonTile> Tiles { get; private set; }
    
        public Location(in List<RoomData> rooms, in HashSet<DungeonTile> corridorTiles, in HashSet<DungeonTile> wallTiles)
        {
            Rooms = rooms;
            Tiles = new HashSet<DungeonTile>();

            foreach (RoomData roomData in rooms)
                Tiles.UnionWith(roomData.GetTiles());

            Tiles.UnionWith(corridorTiles);
            Tiles.UnionWith(wallTiles);
        }
    }
}