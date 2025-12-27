using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class BoxedRoomGenerator : IRoomGenerator
    {
        private readonly Vector2Int m_halfSize;
        private readonly int m_xSize;
        private readonly int m_ySize;

        public BoxedRoomGenerator(in int xSize, in int ySize)
        {
            m_xSize = xSize;
            m_ySize = ySize;
            m_halfSize = new Vector2Int(xSize / 2, ySize / 2);
        }

        public void GenerateRoom(in RoomModel roomModel)
        {
            Vector2Int startPoint = roomModel.WorldPosition - m_halfSize;

            for (int x = 0; x < m_xSize; ++x)
                for (int y = 0; y < m_ySize; ++y)
                    roomModel.AddTile(startPoint + new Vector2Int(x, y), TileType.Floor);
        }
    }
}