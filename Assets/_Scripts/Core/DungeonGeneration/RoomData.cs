using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        private readonly BoundsInt m_roomSpace;
        public BoundsInt RoomSpace => m_roomSpace;
        public Vector2Int WorldPosition { get; private set; }
        public int LeftBorder { get; private set; }
        public int RightBorder { get; private set; }
        public int TopBorder { get; private set; }
        public int BottomBorder { get; private set; }

        public RoomData(in BoundsInt roomSpace, in int xBorderOffset, in int yBorderOffset)
        {
            m_roomSpace = roomSpace;
            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            RightBorder = m_roomSpace.xMax - 1 - xBorderOffset;
            TopBorder = m_roomSpace.yMax - 1 - yBorderOffset;
            LeftBorder = m_roomSpace.xMin + xBorderOffset;
            BottomBorder = m_roomSpace.yMin + yBorderOffset;
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