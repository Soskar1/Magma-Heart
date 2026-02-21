using MagmaHeart.AI.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public class ActionPreviewInstaller : IInstaller
    {
        private readonly CombatTilemapRenderer m_combatTilemapRenderer;
        private ActionPreviewProvider m_actionPreviewProvider;
        private CombatActionPreviewPresenter m_combatActionPreviewPresenter;

        public ActionPreviewInstaller(CombatTilemapRenderer renderer)
        {
            m_combatTilemapRenderer = renderer;
        }

        public IActionPreviewProvider Install(ActionDatabase database, Battle battle, GameWorld gameWorld)
        {
            ActionSelector actionSelectorChain = new AttackActionSelector(database.Get<AttackAction>());
            actionSelectorChain.Next = new MovementActionSelector(database.Get<MovementAction>());
            ActionPreviewService previewService = new ActionPreviewService(actionSelectorChain);

            m_actionPreviewProvider = new ActionPreviewProvider(previewService, battle);

            m_combatTilemapRenderer.Initialize(gameWorld.RoomGrid);

            m_combatActionPreviewPresenter = new CombatActionPreviewPresenter(gameWorld, m_actionPreviewProvider, m_combatTilemapRenderer);

            return m_actionPreviewProvider;
        }

        public void Dispose()
        {
            m_actionPreviewProvider.Disable();
            m_combatActionPreviewPresenter.Disable();
        }
    }
}
