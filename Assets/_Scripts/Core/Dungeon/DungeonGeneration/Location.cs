using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomTileData> Rooms { get; private set; }
        public HashSet<DungeonTile> Tiles { get; private set; }
    
        public Location(in List<RoomTileData> rooms, in HashSet<DungeonTile> corridorTiles, in HashSet<DungeonTile> wallTiles)
        {
            Rooms = rooms;
            Tiles = new HashSet<DungeonTile>();

            foreach (RoomTileData RoomTileData in rooms)
                Tiles.UnionWith(RoomTileData.GetTiles());

            Tiles.UnionWith(corridorTiles);
            Tiles.UnionWith(wallTiles);
        }
    }
}