using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using UnityEngine.UI;

namespace MagmaHeart.Core.Input.Mouse
{
    public class HoverModeController
    {
        private readonly MouseHoverEngine m_engine;

        private readonly UIMouseHoverStrategy m_chainHead;
        private readonly RaycastMouseHoverStrategy m_raycast;
        private readonly TileMouseHoverStrategy m_tile;

        private readonly IHoverHandler m_actionHandler;
        private readonly IHoverHandler m_combatHandler;

        public HoverModeController(MouseHoverEngine engine, Battle battle, GraphicRaycaster raycaster, IActionPreviewProvider actionPreviewProvider)
        {
            m_engine = engine;

            m_chainHead = new UIMouseHoverStrategy(raycaster);
            m_raycast = new RaycastMouseHoverStrategy();
            m_tile = new TileMouseHoverStrategy(battle);

            m_actionHandler = new ActionHoverHandler();
            m_combatHandler = new CombatHoverHandler(battle, actionPreviewProvider);
        }

        public void Disable() => m_engine.Disable();
        
        public void UseRaycastHover()
        {
            m_chainHead.Next = m_raycast;

            m_engine.SetStrategyChain(m_chainHead);
            m_engine.SetHandler(m_actionHandler);
        }

        public void UseTileHover()
        {
            m_chainHead.Next = m_tile;

            m_engine.SetStrategyChain(m_chainHead);
            m_engine.SetHandler(m_combatHandler);
        }
    }
}
