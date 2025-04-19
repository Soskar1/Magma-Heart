using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class CorridorEntrance
    {
        public RoomTileData RoomTileData { get; private set; }
        public Vector2Int StartPoint { get; private set; }

        public CorridorEntrance(RoomTileData roomTileData, Vector2Int startPoint)
        {
            RoomTileData = roomTileData;
            StartPoint = startPoint;
        }
    }

    public class Corridor
    {
        public TileData TileData { get; private set; }

        public CorridorEntrance Entrance1 { get; private set; }
        public CorridorEntrance Entrance2 { get; private set; }

        public HashSet<DungeonTile> BlockingTiles { get; private set; }

        public Corridor(CorridorEntrance entrance1, CorridorEntrance entrance2)
        {
            Entrance1 = entrance1;
            Entrance2 = entrance2;
            TileData = new TileData();
            BlockingTiles = new HashSet<DungeonTile>();
        }
    }
}