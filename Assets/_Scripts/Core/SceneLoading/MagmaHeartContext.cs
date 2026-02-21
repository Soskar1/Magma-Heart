using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.Services;
using MagmaHeart.Core.TutorialSystem;

namespace MagmaHeart.Core.SceneLoading
{
    public record MagmaHeartContext(
        GameWorld World,
        WorldPresenter RoomRenderer,
        Entity Player,
        MagmaHeartServices Services,
        CameraController CameraController,
        BattleContext BattleContext,
        GameUI UI,
        RewardService RewardService,
        TutorialContext Tutorial);
}
