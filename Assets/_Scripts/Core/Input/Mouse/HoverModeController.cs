using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine.UI;

namespace MagmaHeart.Core.Input.Mouse
{
    public class HoverModeController
    {
        private readonly MouseHoverEngine m_engine;

        private readonly MouseHoverStrategy m_actionStrategy;
        private readonly MouseHoverStrategy m_combatStrategy;

        private readonly IHoverHandler m_actionHandler;
        private readonly IHoverHandler m_combatHandler;

        public HoverModeController(MouseHoverEngine engine, PlayerTurnContext turnContext, GraphicRaycaster raycaster)
        {
            m_engine = engine;
            m_actionStrategy = new RaycastMouseHoverStrategy();

            var uiStrategy = new UIMouseHoverStrategy(raycaster);
            uiStrategy.Next = new TileMouseHoverStrategy(turnContext);
            m_combatStrategy = uiStrategy;

            m_actionHandler = new ActionHoverHandler();
            m_combatHandler = new CombatHoverHandler(turnContext);
        }

        public void Disable() => m_engine.Disable();
        
        public void UseRaycastHover()
        {
            m_engine.SetStrategyChain(m_actionStrategy);
            m_engine.SetHandler(m_actionHandler);
        }

        public void UseTileHover()
        {
            m_engine.SetStrategyChain(m_combatStrategy);
            m_engine.SetHandler(m_combatHandler);
        }
    }
}
