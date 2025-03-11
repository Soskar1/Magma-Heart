using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGraphCreator
    {
        private readonly HashSet<RoomData> m_rooms;

        public LocationGraphCreator(HashSet<RoomData> rooms) => m_rooms = rooms;

        public LocationGraph CreateGraph()
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();

            foreach (RoomData roomData in m_rooms)
                edges.UnionWith(CreateEdges(roomData));

            return new LocationGraph(m_rooms, edges);
        }

        private HashSet<RoomConnectionEdge> CreateEdges(in RoomData roomData)
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();
            BoundsInt roomSpace = roomData.RoomSpace;

            foreach (RoomData otherRoom in m_rooms)
            {
                BoundsInt otherRoomSpace = otherRoom.RoomSpace;

                if ((Mathf.Max(roomSpace.xMin, otherRoomSpace.xMin) < Mathf.Min(roomSpace.xMax, otherRoomSpace.xMax) && (roomSpace.yMax == otherRoomSpace.yMin || otherRoomSpace.yMax == roomSpace.yMin)) ||
                    (Mathf.Max(roomSpace.yMin, otherRoomSpace.yMin) < Mathf.Min(roomSpace.yMax, otherRoomSpace.yMax) && (roomSpace.xMax == otherRoomSpace.xMin || otherRoomSpace.xMax == roomSpace.xMin)))
                {
                    RoomConnectionEdge edge = new RoomConnectionEdge(roomData, otherRoom);
                    edges.Add(edge);
                }
            }

            return edges;
        }
    }
}