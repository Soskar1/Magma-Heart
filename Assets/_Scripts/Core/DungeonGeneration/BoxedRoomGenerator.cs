using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class BoxedRoomGenerator : IRoomGenerator
    {
        private readonly RoomData m_roomData;
        private readonly Vector2Int m_halfSize;
        private readonly int m_xSize;
        private readonly int m_ySize;

        public BoxedRoomGenerator(in RoomData roomData, in int xSize, in int ySize)
        {
            m_roomData = roomData;
            m_xSize = xSize;
            m_ySize = ySize;
            m_halfSize = new Vector2Int(xSize / 2, ySize / 2);
        }

        public HashSet<Vector2Int> GenerateRoom(in HashSet<Vector2Int> generatedTiles)
        {
            HashSet<Vector2Int> tiles = new HashSet<Vector2Int>() { m_roomData.WorldPosition };
            Vector2Int startPoint = m_roomData.WorldPosition - m_halfSize;

            if (generatedTiles != null)
                tiles = generatedTiles;

            for (int x = 0; x < m_xSize; ++x)
            {
                for (int y = 0; y < m_ySize; ++y)
                {
                    Vector2Int position = startPoint + new Vector2Int(x, y);
                    if (IsInRoomSpace(position))
                        tiles.Add(position);
                }
            }

            return tiles;
        }

        private bool IsInRoomSpace(in Vector2Int position)
        {
            return position.x < m_roomData.RightBorder && position.x > m_roomData.LeftBorder &&
                position.y < m_roomData.UpperBorder && position.y > m_roomData.BottomBorder;
        }
    }
}