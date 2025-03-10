using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGraph
    {
        private readonly List<RoomData> m_rooms;
        private readonly Dictionary<RoomData, List<RoomData>> m_roomNeighbours;

        public LocationGraph()
        {
            m_rooms = new List<RoomData>();
            m_roomNeighbours = new Dictionary<RoomData, List<RoomData>>();
        }

        public void AddRoomNode(in RoomData roomData)
        {
            AddNeighboursIfExists(roomData);
            m_rooms.Add(roomData);
        }

        private void AddNeighboursIfExists(in RoomData roomToAnalyze)
        {
            List<RoomData> neighbourRooms = new List<RoomData>();
            BoundsInt roomSpace = roomToAnalyze.RoomSpace;

            foreach (RoomData otherRoom in m_rooms)
            {
                BoundsInt otherRoomSpace = otherRoom.RoomSpace;

                if ((Mathf.Max(roomSpace.xMin, otherRoomSpace.xMin) < Mathf.Min(roomSpace.xMax, otherRoomSpace.xMax) && (roomSpace.yMax == otherRoomSpace.yMin || otherRoomSpace.yMax == roomSpace.yMin)) ||
                    (Mathf.Max(roomSpace.yMin, otherRoomSpace.yMin) < Mathf.Min(roomSpace.yMax, otherRoomSpace.yMax) && (roomSpace.xMax == otherRoomSpace.xMin || otherRoomSpace.xMax == roomSpace.xMin)))
                {
                    m_roomNeighbours[otherRoom].Add(roomToAnalyze);
                    neighbourRooms.Add(otherRoom);
                }
            }

            m_roomNeighbours.Add(roomToAnalyze, neighbourRooms);
        }
    }
}