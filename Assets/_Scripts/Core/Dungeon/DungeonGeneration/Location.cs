using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomTileData> Rooms { get; private set; }
        public List<Corridor> Corridors { get; private set; }
        public HashSet<DungeonTile> Tiles { get; private set; }
    
        public Location(in List<RoomTileData> rooms, in List<Corridor> corridors)
        {
            Rooms = rooms;
            Tiles = new HashSet<DungeonTile>();

            foreach (RoomTileData RoomTileData in rooms)
                Tiles.UnionWith(RoomTileData.GetTiles());

            foreach (Corridor corridor in corridors)
                Tiles.UnionWith(corridor.Tiles);
        }
    }
}