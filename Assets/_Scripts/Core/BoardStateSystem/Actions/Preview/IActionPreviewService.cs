using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public interface IActionPreviewService
    {
        public ActionPreview Preview(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile);
    }
}
