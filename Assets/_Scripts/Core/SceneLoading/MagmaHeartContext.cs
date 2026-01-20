using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.Services;
using MagmaHeart.Core.TutorialSystem;

namespace MagmaHeart.Core.SceneLoading
{
    public record MagmaHeartContext(
        DungeonController DungeonController,
        RoomRenderer RoomRenderer,
        Entity Player,
        HoverModeController HoverModeController,
        MagmaHeartServices Services,
        CameraController CameraController,
        BattleContext BattleContext,
        GameUI UI,
        RewardService RewardService,
        TutorialContext Tutorial);
}
