namespace MagmaHeart.DungeonGeneration
{
    public interface IRoomGenerator
    {
        public void GenerateRoom(in RoomModel roomTileData);
    }
}