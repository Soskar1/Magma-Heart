using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : Entity
    {
        public PlayerCombatController CombatController { get; private set; }

        public void Initialize(MouseListener mouseListener, RoomGrid grid, IActionPreviewService previewService)
        {
            base.Initialize(grid, true);

            PlayerTurnContext turnContext = new PlayerTurnContext(Model);
            TurnContext = turnContext;
            CombatController = new PlayerCombatController(turnContext, mouseListener, previewService);
        }

        private void Update() => Animation.PlayAnimations();
    }
}