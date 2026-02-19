using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public interface IActionPreviewService
    {
        public ActionPreview Preview(Room room, EntityModel executor, RoomTile selectedTile);
    }
}
