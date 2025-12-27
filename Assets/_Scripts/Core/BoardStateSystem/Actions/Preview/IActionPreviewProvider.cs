using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public interface IActionPreviewProvider
    {
        public ActionPreview Preview(RoomTile tile);
    }
}
