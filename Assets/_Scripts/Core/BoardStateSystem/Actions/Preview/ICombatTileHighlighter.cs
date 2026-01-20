using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public interface ICombatTileHighlighter
    {
        public void Show(RoomTile tile, bool isValid);
        public void Hide(RoomTile tile);
        public void Clear();
    }
}
