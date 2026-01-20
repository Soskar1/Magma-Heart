using MagmaHeart.DungeonGeneration;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal static class RoomPresets
    {
        public static RoomModel CreateEmptyRoom(BoardDimensions dimensions)
        {
            RoomModel roomModel = new RoomModel(new BoundsInt(dimensions.StartPoint.ToVector3Int(), dimensions.EndPoint.ToVector3Int()));
            for (int x = dimensions.StartPoint.x; x < dimensions.EndPoint.x; ++x)
            {
                for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y; ++y)
                    roomModel.AddTile(new Vector2Int(x, y), TileType.Floor);
            }

            return roomModel;
        }
    }
}