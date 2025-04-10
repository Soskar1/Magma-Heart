using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGraphCreator
    {
        private readonly HashSet<RoomTileData> m_rooms;

        public LocationGraphCreator(HashSet<RoomTileData> rooms) => m_rooms = rooms;

        public LocationGraph CreateGraph()
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();

            foreach (RoomTileData RoomTileData in m_rooms)
                edges.UnionWith(CreateEdges(RoomTileData));

            return new LocationGraph(m_rooms, edges);
        }

        private HashSet<RoomConnectionEdge> CreateEdges(in RoomTileData RoomTileData)
        {
            HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();
            BoundsInt roomSpace = RoomTileData.RoomSpace;

            foreach (RoomTileData otherRoom in m_rooms)
            {
                BoundsInt otherRoomSpace = otherRoom.RoomSpace;

                if ((Mathf.Max(roomSpace.xMin, otherRoomSpace.xMin) < Mathf.Min(roomSpace.xMax, otherRoomSpace.xMax) && (roomSpace.yMax == otherRoomSpace.yMin || otherRoomSpace.yMax == roomSpace.yMin)) ||
                    (Mathf.Max(roomSpace.yMin, otherRoomSpace.yMin) < Mathf.Min(roomSpace.yMax, otherRoomSpace.yMax) && (roomSpace.xMax == otherRoomSpace.xMin || otherRoomSpace.xMax == roomSpace.xMin)))
                {
                    RoomConnectionEdge edge = new RoomConnectionEdge(RoomTileData, otherRoom);
                    edges.Add(edge);
                }
            }

            return edges;
        }
    }
}