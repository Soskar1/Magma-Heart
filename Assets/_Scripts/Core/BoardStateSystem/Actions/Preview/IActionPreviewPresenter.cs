using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public interface IActionPreviewPresenter
    {
        void Present(ActionPreview preview, RoomTile tile, CombatBoardState state);
    }
}
