using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomTileData> Rooms { get; private set; }
        public List<Corridor> Corridors { get; private set; }
        public HashSet<DungeonTile> Tiles { get; private set; }
        public HashSet<DungeonTile> FloorTiles { get; private set; }
        public HashSet<DungeonTile> WallTiles { get; private set; }
        public HashSet<DungeonTile> CorridorEntranceTiles { get; private set; }
    
        public Location(in List<RoomTileData> rooms, in List<Corridor> corridors)
        {
            Rooms = rooms;

            Tiles = new HashSet<DungeonTile>();
            FloorTiles = new HashSet<DungeonTile>();
            WallTiles = new HashSet<DungeonTile>();
            CorridorEntranceTiles = new HashSet<DungeonTile>();

            foreach (RoomTileData roomTileData in rooms)
            {
                HashSet<DungeonTile> tiles = roomTileData.GetTiles();
                Tiles.UnionWith(tiles);
                FloorTiles.UnionWith(tiles.Where(t => t.Type == TileType.Floor));
                WallTiles.UnionWith(tiles.Where(t => t.Type == TileType.Wall));
            }

            foreach (Corridor corridor in corridors)
            {
                HashSet<DungeonTile> tiles = corridor.TileData.GetTiles();
                Tiles.UnionWith(tiles);
                FloorTiles.UnionWith(tiles.Where(t => t.Type == TileType.Floor));
                WallTiles.UnionWith(tiles.Where(t => t.Type == TileType.Wall));
                CorridorEntranceTiles.UnionWith(corridor.BlockingTiles);
            }
        }
    }
}