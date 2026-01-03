using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;

namespace MagmaHeart.Core.SceneLoading
{
    public record MagmaHeartContext(
        DungeonController DungeonController,
        RoomRenderer RoomRenderer,
        Entity Player,
        HoverModeController HoverModeController,
        EntityMovementService EntityMovementService,
        CameraController CameraController,
        Battle Battle,
        BattleReward BattleReward,
        GameUI UI
        );
}
