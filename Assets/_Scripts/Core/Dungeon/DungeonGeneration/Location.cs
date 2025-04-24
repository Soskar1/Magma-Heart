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

        private readonly LocationGraph m_locationGraph;
    
        public Location(in List<RoomTileData> rooms, in List<Corridor> corridors, in LocationGraph locationGraph)
        {
            Rooms = rooms;
            Corridors = corridors;
            m_locationGraph = locationGraph;

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

        public RoomTileData GetFarthestRoomFrom(RoomTileData roomTileData)
        {
            RoomTileData farthestRoom = roomTileData;
            HashSet<RoomTileData> visited = new HashSet<RoomTileData>();
            Queue<RoomTileData> queue = new Queue<RoomTileData>();
            queue.Enqueue(roomTileData);

            while (queue.Any())
            {
                RoomTileData room = queue.Dequeue();
                visited.Add(room);
                farthestRoom = room;

                HashSet<RoomTileData> neighbours = m_locationGraph.ConnectedRooms[room];
                foreach (RoomTileData neighbor in neighbours)
                    if (!visited.Contains(neighbor) && !queue.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return farthestRoom;
        }
    }
}