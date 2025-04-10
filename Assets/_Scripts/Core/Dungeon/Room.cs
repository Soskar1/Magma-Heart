using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class Room : MonoBehaviour
    {
        public RoomTileData RoomTiles { get; private set; }
        public Vector2Int WorldPosition => RoomTiles.WorldPosition;

        public Room(RoomTileData tileData)
        {
            RoomTiles = tileData;
        }
    }
}