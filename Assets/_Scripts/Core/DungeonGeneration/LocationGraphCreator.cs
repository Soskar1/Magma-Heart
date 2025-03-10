using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGraphCreator
    {
        private readonly List<RoomData> m_rooms;

        public LocationGraphCreator(List<RoomData> rooms) => m_rooms = rooms;

        public LocationGraph CreateGraph()
        {
            Dictionary<RoomData, List<RoomConnectionEdge>> edges = new Dictionary<RoomData, List<RoomConnectionEdge>>();

            foreach (RoomData roomData in m_rooms)
            {
                List<RoomConnectionEdge> edgesToNeighbours = CreateEdges(roomData);
                edges.Add(roomData, edgesToNeighbours);
            }

            return new LocationGraph(m_rooms, edges);
        }

        private List<RoomConnectionEdge> CreateEdges(in RoomData roomData)
        {
            List<RoomConnectionEdge> edges = new List<RoomConnectionEdge>();
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