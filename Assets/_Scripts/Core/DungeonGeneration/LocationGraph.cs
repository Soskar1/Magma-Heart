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
        private readonly List<RoomData> m_nodes;
        private readonly Dictionary<RoomData, List<RoomConnectionEdge>> m_edges;

        public LocationGraph(in List<RoomData> nodes, in Dictionary<RoomData, List<RoomConnectionEdge>> edges)
        {
            m_nodes = nodes;
            m_edges = edges;
        }
    }
}