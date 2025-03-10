using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomConnectionEdge
    {
        public float Cost { get; private set; }
        public RoomData First { get; private set; }
        public RoomData Second { get; private set; }

        public RoomConnectionEdge(in RoomData first, in RoomData second)
        {
            First = first;
            Second = second;
            Cost = Vector2Int.Distance(first.WorldPosition, second.WorldPosition);
        }
    }

    public class LocationGraph
    {
        public List<RoomData> Nodes { get; private set; }
        public Dictionary<RoomData, List<RoomConnectionEdge>> Edges { get; private set; }

        public LocationGraph(in List<RoomData> nodes, in Dictionary<RoomData, List<RoomConnectionEdge>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}