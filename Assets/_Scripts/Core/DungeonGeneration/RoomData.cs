using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        private readonly BoundsInt m_roomSpace;
        public BoundsInt RoomSpace => m_roomSpace;
        public Vector2Int WorldPosition { get; private set; }
        public int LeftBorder => m_roomSpace.xMin;
        public int RightBorder { get; private set; }
        public int TopBorder { get; private set; }
        public int BottomBorder => m_roomSpace.yMin;

        public RoomData(in BoundsInt roomSpace)
        {
            m_roomSpace = roomSpace;
            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            RightBorder = m_roomSpace.xMax - 1;
            TopBorder = m_roomSpace.yMax - 1;
        }

        public Vector2Int ToRoomSpace(Vector2Int position)
        {
            if (position.x > RightBorder)
                position.x = RightBorder;

            if (position.x < LeftBorder)
                position.x = LeftBorder;

            if (position.y > TopBorder)
                position.y = TopBorder;

            if (position.y < BottomBorder)
                position.y = BottomBorder;

            return position;
        }
    }
}