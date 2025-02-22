using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        public Vector2Int WorldPosition { get; private set; }
        public int LeftBorder { get; private set; }
        public int RightBorder { get; private set; }
        public int UpperBorder { get; private set; }
        public int BottomBorder { get; private set; }

        public RoomData(in Vector2Int worldPosition, in int xBorderSize, in int yBorderSize)
        {
            WorldPosition = worldPosition;
            LeftBorder = WorldPosition.x - xBorderSize / 2;
            RightBorder = WorldPosition.x + xBorderSize / 2;
            BottomBorder = WorldPosition.y - yBorderSize / 2;
            UpperBorder = WorldPosition.y + yBorderSize / 2;
        }
    }
}