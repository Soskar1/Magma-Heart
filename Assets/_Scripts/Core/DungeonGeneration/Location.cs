using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomData> Rooms { get; private set; }
        public HashSet<DungeonTile> CorridorTiles { get; private set; }
    
        public Location(in List<RoomData> rooms, in HashSet<DungeonTile> corridorTile)
        {
            Rooms = rooms;
            CorridorTiles = corridorTile;
        }
    }
}