using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomModel> Rooms { get; private set; }
        public HashSet<DungeonTile> Tiles { get; private set; }
        public HashSet<DungeonTile> FloorTiles { get; private set; }
        public HashSet<DungeonTile> WallTiles { get; private set; }
    
        public Location(in List<RoomModel> rooms)
        {
            Rooms = rooms;

            Tiles = new HashSet<DungeonTile>();
            FloorTiles = new HashSet<DungeonTile>();
            WallTiles = new HashSet<DungeonTile>();

            foreach (RoomModel roomTileData in rooms)
            {
                HashSet<DungeonTile> tiles = roomTileData.GetTiles();
                Tiles.UnionWith(tiles);
                FloorTiles.UnionWith(tiles.Where(t => t.Type == TileType.Floor));
                WallTiles.UnionWith(tiles.Where(t => t.Type == TileType.Wall));
            }
        }
    }
}