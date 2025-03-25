using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class Location
    {
        public List<RoomData> Rooms { get; private set; }
        public HashSet<Vector2Int> CorridorTiles { get; private set; }
    
        public Location(in List<RoomData> rooms, in HashSet<Vector2Int> corridorTile)
        {
            Rooms = rooms;
            CorridorTiles = corridorTile;
        }
    }
}