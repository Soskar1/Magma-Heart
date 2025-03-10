using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomNode
    {
        public RoomData RoomData { get; private set; }
        private readonly List<RoomNode> m_connectedRooms;

        public RoomNode(in RoomData roomData) => RoomData = roomData;

        public void ConnectRoom(in RoomNode roomNode) => m_connectedRooms.Add(roomNode);
    }

    public class LocationGraph
    {
        private readonly List<RoomNode> m_nodes;
        private readonly Dictionary<RoomData, List<RoomData>> m_roomNeighbours;

        public LocationGraph()
        {
            m_nodes = new List<RoomNode>();
            m_roomNeighbours = new Dictionary<RoomData, List<RoomData>>();
        }

        public void AddRoomNode(in RoomData roomData)
        {
            RoomNode node = new RoomNode(roomData);
            AddNeighboursIfExists(roomData);

            m_nodes.Add(node);
        }

        private void AddNeighboursIfExists(in RoomData roomToAnalyze)
        {
            List<RoomData> neighbourRooms = new List<RoomData>();
            BoundsInt roomSpace = roomToAnalyze.RoomSpace;

            foreach (RoomNode node in m_nodes)
            {
                RoomData otherRoomData = node.RoomData;
                BoundsInt otherRoomSpace = otherRoomData.RoomSpace;

                if ((Mathf.Max(roomSpace.xMin, otherRoomSpace.xMin) < Mathf.Min(roomSpace.xMax, otherRoomSpace.xMax) && (roomSpace.yMax == otherRoomSpace.yMin || otherRoomSpace.yMax == roomSpace.yMin)) ||
                    (Mathf.Max(roomSpace.yMin, otherRoomSpace.yMin) < Mathf.Min(roomSpace.yMax, otherRoomSpace.yMax) && (roomSpace.xMax == otherRoomSpace.xMin || otherRoomSpace.xMax == roomSpace.xMin)))
                {
                    m_roomNeighbours[otherRoomData].Add(roomToAnalyze);
                    neighbourRooms.Add(otherRoomData);
                }
            }

            m_roomNeighbours.Add(roomToAnalyze, neighbourRooms);
        }
    }
}