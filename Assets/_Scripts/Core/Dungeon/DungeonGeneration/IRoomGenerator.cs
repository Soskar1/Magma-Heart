namespace MagmaHeart.Core.Dungeon
{
    public interface IRoomGenerator
    {
        public void GenerateRoom(in RoomModel roomTileData);
    }
}