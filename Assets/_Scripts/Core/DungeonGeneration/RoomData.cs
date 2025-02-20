using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        public Vector2Int WorldPosition { get; private set; }
        private readonly int m_leftBorder;
        private readonly int m_rightBorder;
        private readonly int m_upperBorder;
        private readonly int m_bottomBorder;

        public RoomData(in Vector2Int worldPosition, in int xBorderSize, in int yBorderSize)
        {
            WorldPosition = worldPosition;
            m_leftBorder = WorldPosition.x - xBorderSize / 2;
            m_rightBorder = WorldPosition.x + xBorderSize / 2;
            m_bottomBorder = WorldPosition.y - yBorderSize / 2;
            m_upperBorder = WorldPosition.y + yBorderSize / 2;
        }

        public bool IsInRoomSpace(in Vector2Int position)
        {
            return position.x < m_rightBorder && position.x > m_leftBorder &&
                position.y < m_upperBorder && position.y > m_bottomBorder;
        }
    }
}