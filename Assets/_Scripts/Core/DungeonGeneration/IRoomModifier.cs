namespace MagmaHeart.Core.Dungeon
{
    public interface IRoomModifier
    {
        public void ModifyRoom(in RoomData roomData);
    }
}