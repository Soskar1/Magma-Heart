using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGraphCreator
    {
        private readonly List<RoomTileData> m_rooms;

        public LocationGraphCreator(List<RoomTileData> rooms) => m_rooms = rooms;

        public LocationGraph CreateGraph()
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();

            foreach (RoomTileData roomTileData in m_rooms)
                edges.UnionWith(CreateEdges(roomTileData));

            return new LocationGraph(m_rooms.ToHashSet(), edges);
        }

        private HashSet<RoomConnectionEdge> CreateEdges(in RoomTileData roomTileData)
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();
            BoundsInt roomSpace = roomTileData.RoomSpace;

            foreach (RoomTileData otherRoom in m_rooms)
            {
                BoundsInt otherRoomSpace = otherRoom.RoomSpace;

                if ((Mathf.Max(roomSpace.xMin, otherRoomSpace.xMin) < Mathf.Min(roomSpace.xMax, otherRoomSpace.xMax) && (roomSpace.yMax == otherRoomSpace.yMin || otherRoomSpace.yMax == roomSpace.yMin)) ||
                    (Mathf.Max(roomSpace.yMin, otherRoomSpace.yMin) < Mathf.Min(roomSpace.yMax, otherRoomSpace.yMax) && (roomSpace.xMax == otherRoomSpace.xMin || otherRoomSpace.xMax == roomSpace.xMin)))
                {
                    RoomConnectionEdge edge = new RoomConnectionEdge(roomTileData, otherRoom);
                    edges.Add(edge);
                }
            }

            return edges;
        }
    }
}