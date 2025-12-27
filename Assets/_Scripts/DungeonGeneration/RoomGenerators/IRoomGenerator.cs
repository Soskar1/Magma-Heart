namespace MagmaHeart.DungeonGeneration.RoomGeneration
{
    public interface IRoomGenerator
    {
        public void GenerateRoom(in RoomModel roomTileData);
    }
}